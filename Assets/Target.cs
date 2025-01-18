using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    public Door door;

    private void OnTriggerEnter2D(Collider2D collision)
    {
    }
    public void onHit()
    {
        door.open();
        Destroy(gameObject);
    }
}
