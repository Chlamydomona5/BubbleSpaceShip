using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;

public class ComposeBubbleBase : MonoBehaviour
{
    protected bool OnMouseControl;
    protected Rigidbody2D Rigidbody2D;
    protected Collider2D Collider2D;
    protected SpriteRenderer SpriteRenderer;
    
    [SerializeField, ReadOnly] protected BubbleShip bubbleShip;
    public BubbleShip BubbleShip => bubbleShip;
    
    private void Awake()
    {
        Rigidbody2D = GetComponent<Rigidbody2D>();
        Collider2D = GetComponent<Collider2D>();
        SpriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (OnMouseControl)
        {
            var pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            pos = new Vector3(pos.x, pos.y, 0);
            Rigidbody2D.MovePosition(pos);
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

    public void DisableSelfPhysics()
    {
        OnMouseControl = false;
        Destroy(Rigidbody2D);
    }
}
