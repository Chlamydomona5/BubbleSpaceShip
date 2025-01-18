using System;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using UnityEngine;

public class BubbleShip : MonoBehaviour
{
    public ComposeBubbleBase coreBubble;
    
    public List<ComposeBubbleBase> components;
    public List<(ComposeBubbleBase connectionA, ComposeBubbleBase connectionB)> Connections;
    
    public bool onMouseControl;
    public bool onMove;
    
    private Rigidbody2D _rigidbody;

    [SerializeField] private float targetYSpeed = 2f;
    [SerializeField] private float normalAccerlation = 1f;
    
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        
        DontDestroyOnLoad(transform.parent.gameObject);
    }
    
    public void ExplodeBubbleAt(Vector2 position)
    {
        
    }
    
    public void AddForceToShip(Vector2 force)
    {
        
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
            _rigidbody.MovePosition(pos);
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
    }

    private void OnMouseDown()
    {
        if(!onMove)
            onMouseControl = true;
    }
    
    private void OnMouseUp()
    {
        if(!onMove)
            onMouseControl = false;
    }

    public void StartActualMove()
    {
        onMove = true;
    }
}