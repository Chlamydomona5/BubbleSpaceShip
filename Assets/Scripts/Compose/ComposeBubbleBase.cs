using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;

public class ComposeBubbleBase : MonoBehaviour
{
    [SerializeField] private BubbleData data;

    [SerializeField] private Vector2 releaseForce;
    
    private bool _onMouseControl;
    private Rigidbody2D _rigidbody2D;
    private Collider2D _collider2D;

    [SerializeField, ReadOnly] private float size;

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

    public void Blow(float delta)
    {
        size += delta;
        transform.localScale = Vector3.one * size;
        transform.position += Vector3.up * delta / 2f;
    }

    public void Release()
    {
        _rigidbody2D.bodyType = RigidbodyType2D.Dynamic;
        _rigidbody2D.AddForce(releaseForce);
    }
}
