using Sirenix.OdinInspector;
using UnityEngine;

public class GeneratedBubble : ComposeBubbleBase
{
    [SerializeField] private BubbleData data;
    [SerializeField, ReadOnly] private float size;
    [SerializeField] private Vector2 releaseForce;
    
    public void Init(BubbleData data, float size)
    {
        this.data = data;
        if(data.sprite)
            SpriteRenderer.sprite = data.sprite;
        SpriteRenderer.color = data.color;
        this.size = size;
        transform.localScale = Vector3.one * size;
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
    
    public override void Explode()
    {
        Debug.Log("Explode");
        data.ExplodeEffect.Effect(bubbleShip, transform.position, size);
        Destroy(gameObject);
    }
}