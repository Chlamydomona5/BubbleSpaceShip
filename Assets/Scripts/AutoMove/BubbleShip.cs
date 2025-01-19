using System;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

public class BubbleShip : MonoBehaviour
{
    public ComposeBubbleBase coreBubble;
    
    public List<ComposeBubbleBase> components;
    public List<(ComposeBubbleBase connectionA, ComposeBubbleBase connectionB)> Connections;
    
    public bool onMouseControl;
    public bool onMove;
    private Vector2 _mouseRelativePosition;
    
    private Rigidbody2D _rigidbody;

    [SerializeField] private float targetYSpeed = 2f;
    [SerializeField] private float normalAccerlation = 1f;
    [SerializeField] public float pushForceFactor = 10f;

    [SerializeField] private CinemachineVirtualCamera shipCamera;

    public Vector2 previousVelocity;

    [SerializeField] private SpriteRenderer crabBody;
    [SerializeField] private SpriteRenderer crabBodyCry;


    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        Connections = new List<(ComposeBubbleBase, ComposeBubbleBase)>();
        
        DontDestroyOnLoad(transform.parent.gameObject);
    }

    public void ExplodeBubbleAt(GeneratedBubble bubble)
    {
        components.Remove(bubble);
        Connections.RemoveAll(x => x.connectionA == bubble || x.connectionB == bubble);
        var visited = new HashSet<ComposeBubbleBase>();
        var queue = new Queue<ComposeBubbleBase>();

        // Start from coreBubble
        queue.Enqueue(coreBubble);
        visited.Add(coreBubble);

        while (queue.Count > 0)
        {
            var currentBubble = queue.Dequeue();
            foreach (var connection in Connections)
            {
                if (connection.connectionA == currentBubble && !visited.Contains(connection.connectionB))
                {
                    visited.Add(connection.connectionB);
                    queue.Enqueue(connection.connectionB);
                }
                else if (connection.connectionB == currentBubble && !visited.Contains(connection.connectionA))
                {
                    visited.Add(connection.connectionA);
                    queue.Enqueue(connection.connectionA);
                }
            }
        }

        // Find isolated components
        List<ComposeBubbleBase> isolated = new List<ComposeBubbleBase>();
        foreach (var component in components)
        {
            if (!visited.Contains(component))
            {
                isolated.Add(component);
            }
        }

        for (int i = 0; i < isolated.Count; i++)
        {
            isolated[i].Explode(false);
        }
    }

    public void AddForceToShip(Vector2 force)
    {
        Debug.Log("ADD FORCE " + force);
        _rigidbody.AddForce(force, ForceMode2D.Force);
    }
    
    public void AddForceToShipImpulse(Vector2 force)
    {
        Debug.Log("ADD FORCE " + force);
        _rigidbody.AddForce(force, ForceMode2D.Impulse);
    }
    
    public void SetVelocity(Vector2 velocity)
    {
        _rigidbody.velocity = velocity;
    }

    public void RotateShip()
    {
        // Dotween rotate 90
        transform.DOBlendableRotateBy(new Vector3(0, 0, -180), 3f).SetEase(Ease.InSine);
    }

    public void ReceiveBubble(ComposeBubbleBase newBubble, Collision2D other)
    {
        components.Add(newBubble);
        newBubble.DisableSelfPhysics();
        newBubble.transform.SetParent(transform);
        if(newBubble is GeneratedBubble generateBubble && generateBubble.Data.ExplodeEffect is GunEffect)
        {
            // Make New Bubble toward the core
            var direction = (coreBubble.transform.position - newBubble.transform.position).normalized;
            newBubble.transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 90f);
        }
        
        // Handle connection
        foreach (var contact in other.contacts)
        {
            if (contact.collider.GetComponent<ComposeBubbleBase>())
            {
                var otherBubble = contact.collider.GetComponent<ComposeBubbleBase>();
                Connections.Add((newBubble, otherBubble));
            }
        }
    }
    
    private void Update()
    {
        if (onMouseControl)
        {
            var pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            pos = new Vector3(pos.x, pos.y, 0);
            _rigidbody.MovePosition(pos + (Vector3)_mouseRelativePosition);
        }
    }

    private void FixedUpdate()
    {
        if (onMove)
        {
            var targetSpeed = new Vector2(0, targetYSpeed);
            var currentSpeed = _rigidbody.velocity;
        
            var deltaSpeed = targetSpeed - currentSpeed;
            _rigidbody.AddForce(deltaSpeed * normalAccerlation);
        }

        previousVelocity = GetComponent<Rigidbody2D>().velocity;
    }

    private void OnMouseDown()
    {
        if (!onMove)
        {
            onMouseControl = true;
            _mouseRelativePosition = (Vector2)transform.position - (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
        else
        {
            //Judge which component this click is on
            var pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            pos = new Vector3(pos.x, pos.y, 0);
            var hits = Physics2D.RaycastAll(pos, Vector2.up, .1f, LayerMask.GetMask("Bubble"));
            Debug.DrawLine(pos, pos + Vector3.up * 1f, Color.red, 1f);
            foreach (var hit in hits)
            {
                if (hit.collider)
                {
                    var bubble = hit.collider.GetComponent<GeneratedBubble>();
                    bubble?.Explode(true);
                }
            }
        }
    }

    private void OnMouseUp()
    {
        if(!onMove)
        {
            onMouseControl = false;
            _rigidbody.velocity = Vector2.zero;
            _rigidbody.totalTorque = 0;
        }
    }

    public void ActualMove(bool move)
    {
        onMove = move;
        _rigidbody.velocity = Vector2.zero;
        _rigidbody.totalTorque = 0;
        _rigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    public void ResetShip()
    {
        for (int i = 0; i < components.Count; i++)
        {
            if(components[i] && components[i].gameObject)
                Destroy(components[i].gameObject);
        }
        components.Clear();
    }

    public void Stop()
    {
        _rigidbody.velocity = Vector2.zero;
    }

    public void MakeSureSpeedDontChange()
    {
        _rigidbody.velocity = previousVelocity;
    }

    public void LoseMode()
    {
        ResetShip();
        var tran = shipCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
        tran.m_TrackedObjectOffset = Vector3.zero;
        tran.m_DeadZoneHeight = 0;
        tran.m_DeadZoneWidth = 0;
        
        crabBody.gameObject.SetActive(false);
        crabBodyCry.gameObject.SetActive(true);
        // Endless Rotate
        crabBodyCry.transform.DORotate(new Vector3(0, 0, 360), .8f, RotateMode.FastBeyond360).SetEase(Ease.Linear).SetLoops(-1, LoopType.Incremental);
    }
}