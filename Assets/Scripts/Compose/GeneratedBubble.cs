using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;

public class GeneratedBubble : ComposeBubbleBase
{
    [SerializeField] private BubbleData data;
    public BubbleData Data => data;
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
                bubbleShip.ReceiveBubble(this, other);
            }
        }
    }
    
    public override void Explode(bool checkConnection)
    {
        Debug.Log("Explode");
        data.ExplodeEffect.Effect(bubbleShip, transform.position, size);
        if (checkConnection)
        {
            bubbleShip.ExplodeBubbleAt(this);
        }
        
        DestroyImmediate(gameObject);
    }
}