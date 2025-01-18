using Sirenix.OdinInspector;
using UnityEngine;

public class GeneratedBubble : ComposeBubbleBase
{
    [SerializeField] private BubbleData data;
    [SerializeField, ReadOnly] private float size;
    [SerializeField] private Vector2 releaseForce;
    
    public void Init(BubbleData data)
    {
        this.data = data;
    }
    
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
    
    public void Explode()
    {
        Debug.Log("Explode");
        data.ExplodeEffect.Effect(bubbleShip, transform.position, size);
        Destroy(gameObject);
    }
}