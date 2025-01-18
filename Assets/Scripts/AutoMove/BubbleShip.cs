using System;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using DG.Tweening;
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

    [SerializeField] private Camera shipCamera;

    public Vector2 previousVelocity;


    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        
        DontDestroyOnLoad(transform.parent.gameObject);
    }
    
    public void ExplodeBubbleAt(GeneratedBubble bubble)
    {
        
    }
    
    public void AddForceToShip(Vector2 force)
    {
        _rigidbody.AddForce (force,ForceMode2D.Force);
    }
    
    public void RotateShip()
    {
        // Dotween rotate 90
        transform.DOBlendableRotateBy(new Vector3(0, 0, 90), 0.5f).SetEase(Ease.InSine);
    }

    public void ReceiveBubble(ComposeBubbleBase newBubble)
    {
        components.Add(newBubble);
        newBubble.DisableSelfPhysics();
        newBubble.transform.SetParent(transform);
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
            var pos = shipCamera.ScreenToWorldPoint(Input.mousePosition);
            pos = new Vector3(pos.x, pos.y, 0);
            var hits = Physics2D.RaycastAll(pos, Vector2.up, .1f, LayerMask.GetMask("Bubble"));
            Debug.DrawLine(pos, pos + Vector3.up * 1f, Color.red, 1f);
            foreach (var hit in hits)
            {
                if (hit.collider)
                {
                    var bubble = hit.collider.GetComponent<GeneratedBubble>();
                    bubble?.Explode();
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
}