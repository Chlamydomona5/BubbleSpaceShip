using Sirenix.OdinInspector;
using UnityEngine;

public class GeneratedBubble : ComposeBubbleBase
{
    [SerializeField, ReadOnly] private float size;
    [SerializeField] private Vector2 releaseForce;
    
    public void Blow(float delta)
    {
        size += delta;
        transform.localScale = Vector3.one * size;
        transform.position += Vector3.up * delta / 2f;
    }

    public void Release()
    {
        Rigidbody2D.bodyType = RigidbodyType2D.Dynamic;
        Rigidbody2D.AddForce(releaseForce);
    }
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Bubble"))
        {
            if (other.gameObject.GetComponentInParent<BubbleShip>())
            {
                bubbleShip = other.gameObject.GetComponentInParent<BubbleShip>();
                bubbleShip.ReceiveBubble(this);
            }
        }
    }
}