using System;
using UnityEngine;

public class ComposeController : MonoBehaviour
{
    public float crabSpeed = 1f;

    [SerializeField] private GameObject crab;
    [SerializeField] private GameObject craw;

    private void Update()
    {
        var mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPosition = new Vector3(mouseWorldPosition.x, mouseWorldPosition.y, 0);
        // Crab Move
        if(Input.GetKey(KeyCode.A)) crab.transform.position += Vector3.left * crabSpeed * Time.deltaTime;
        if(Input.GetKey(KeyCode.D)) crab.transform.position += Vector3.right * crabSpeed * Time.deltaTime;
        // Craw Move
        craw.transform.position = mouseWorldPosition;
        // Click Handler
        /*if (Input.GetMouseButtonDown(0))
        {
            //RayCast Bubble at mouse position
            var hit = Physics2D.Raycast(mouseWorldPosition, Vector2.up, .1f, LayerMask.GetMask("Bubble"));
            if (hit.collider)
            {
                hit.collider.GetComponent<ComposeBubbleBase>().OnMouseControl(true);
            }
        }*/
    }
}