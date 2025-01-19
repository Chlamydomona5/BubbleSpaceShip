using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    public Door door;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        BubbleShip bs = collision.gameObject.GetComponentInParent<BubbleShip>();
        if(bs != null)
        {
            onHit();
        }
    }

    public void onHit()
    {
        door.open();
        Destroy(gameObject);
    }
}
