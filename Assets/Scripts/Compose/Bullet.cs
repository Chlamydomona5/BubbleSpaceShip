using System.Diagnostics;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public GameObject biuPrefab;
    public void Init(Vector2 speed)
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.velocity = speed;
        Instantiate(biuPrefab,transform.position,Quaternion.identity);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Target tar = collision.gameObject.GetComponent<Target>();
        if (tar != null)
        {
            tar.onHit();
            Destroy(gameObject);
        }
    }
}