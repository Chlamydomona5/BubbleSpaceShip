using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;

public class GeneratedBubble : ComposeBubbleBase
{
    [SerializeField] private BubbleData data;
    public BubbleData Data => data;
    [SerializeField, ReadOnly] private float size;

    public void Init(BubbleData data, float size)
    {
        this.data = data;
        if (data.sprite)
            SpriteRenderer.sprite = data.sprite;
        SpriteRenderer.color = data.color;
        transform.localScale = Vector3.one * size;

        // 获取图片的原始大小
        Sprite sprite = SpriteRenderer.sprite;
        if (sprite == null) return;

        Vector2 boundSize = sprite.bounds.size;

        // 计算缩放比例
        float scaleX = 1f / boundSize.x;
        float scaleY = 1f / boundSize.y;

        // 应用缩放比例，以使图片适应1x1大小
        SpriteRenderer.transform.localScale = new Vector3(scaleX, scaleY, 1f);
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
        
        if(gameObject)
            DestroyImmediate(gameObject);
    }
}