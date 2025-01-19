using DG.Tweening;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;
using Sequence = DG.Tweening.Sequence;

public class GeneratedBubble : ComposeBubbleBase
{
    [SerializeField] private BubbleData data;
    public BubbleData Data => data;
    [SerializeField] private float size;

    [SerializeField] private Animator explodeAnimator;
    private Sequence _destroySequence;

    protected override void Awake()
    {
        base.Awake();
        if(data) Init(data, size);
    }

    public void Init(BubbleData data, float size)
    {
        this.data = data;
        if (data.sprite)
            SpriteRenderer.sprite = data.sprite;
        SpriteRenderer.color = data.color;
        transform.localScale = Vector3.one * size;
        this.size = size;

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
                bubbleShip.MakeSureSpeedDontChange();
            }
        }
    }
    
    public override void Explode(bool checkConnection)
    {
        if(!this || !gameObject) return;

        Debug.Log("Explode");
        var originalScale = SpriteRenderer.transform.localScale;
        DestroyImmediate(Collider2D);
        Sequence sequence = DOTween.Sequence();
        sequence.Append(SpriteRenderer.transform.DOScale(originalScale * 0.75f, 0.1f));
        sequence.Append(SpriteRenderer.transform.DOScale(originalScale * 1.3f, 0.1f));
        sequence.AppendCallback(() =>
        {
            DestroyImmediate(SpriteRenderer);
            explodeAnimator.gameObject.SetActive(true);
            explodeAnimator.Play("Explode");
        });
        sequence.AppendInterval(0.15f);
        sequence.AppendCallback((() => DestroyImmediate(gameObject)));
        
        data.ExplodeEffect.Effect(bubbleShip, transform.position, size);
        if (checkConnection)
        {
            bubbleShip.ExplodeBubbleAt(this);
        }
        
        // ???
        if(!this || !gameObject) return;

    }

    public void Selected()
    {
        if(SpriteRenderer)
            SpriteRenderer.transform.DOScale(SpriteRenderer.transform.localScale * 1.2f, 0.1f);
    }

    public void UnSelected()
    {
        if(SpriteRenderer)
            SpriteRenderer.transform.DOScale(SpriteRenderer.transform.localScale / 1.2f, 0.1f);
    }
}