using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BiuEffect : MonoBehaviour
{
    public SpriteRenderer targetSprite; // 需要控制的Sprite对象
    public float shakeDuration = 1f; // 抖动持续时间
    public float fadeDuration = 2f; // 逐渐消失持续时间
    public float scaleDuration = 0.5f; // 放大持续时间
    public Vector3 targetScale = new Vector3(1.2f, 1.2f, 1f); // 目标放大比例
    void Start()
    {
        targetScale *= transform.localScale.x;
        // 图片出现后在原地抖动并逐渐变为透明
        targetSprite.transform.DOShakePosition(shakeDuration, new Vector3(1, 1, 0), 20, 90, false, true);
        targetSprite.transform.DOScale(targetScale, scaleDuration).SetLoops(2, LoopType.Yoyo);
        targetSprite.DOFade(0, fadeDuration).OnComplete(() =>
        {
            // 完全透明后，图片对象销毁
            Destroy(targetSprite.gameObject);
        });
    }
}