using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BubbleShip : MonoBehaviour
{
    public ComposeBubbleBase coreBubble;
    
    public List<ComposeBubbleBase> components;
    public List<(ComposeBubbleBase connectionA, ComposeBubbleBase connectionB)> Connections;
    public bool OnMouseControl;
    
    private Rigidbody2D _rigidbody;
    
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        
        DontDestroyOnLoad(gameObject);
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
        if (OnMouseControl)
        {
            var pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            pos = new Vector3(pos.x, pos.y, 0);
            _rigidbody.MovePosition(pos);
        }
    }

    private void OnMouseDown()
    {
        OnMouseControl = true;
    }
    
    private void OnMouseUp()
    {
        OnMouseControl = false;
    }
}