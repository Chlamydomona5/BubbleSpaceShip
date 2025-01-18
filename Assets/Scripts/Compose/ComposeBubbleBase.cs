using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ComposeBubbleBase : MonoBehaviour
{
    [SerializeField] private BubbleData data;
    
    private bool _onMouseControl;
    private Rigidbody2D _rigidbody2D;
    private Collider2D _collider2D;

    public void Init(BubbleData data)
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _collider2D = GetComponent<Collider2D>();

        this.data = data;
    }

    private void Update()
    {
        if (_onMouseControl)
        {
            transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector3(transform.position.x, transform.position.y, 0);
        }
    }

    private void OnMouseDown()
    {
        _onMouseControl = true;
    }
    
    private void OnMouseUp()
    {
        _onMouseControl = false;
    }
}
