using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ComposeBubbleBase : MonoBehaviour
{
    private bool _onMouseControl;
    private Rigidbody2D _rigidbody2D;
    private Collider2D _collider2D;

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _collider2D = GetComponent<Collider2D>();
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
