using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BiuEffect : MonoBehaviour
{
    public SpriteRenderer targetSprite; // ��Ҫ���Ƶ�Sprite����
    public float shakeDuration = 1f; // ��������ʱ��
    public float fadeDuration = 2f; // ����ʧ����ʱ��
    public float scaleDuration = 0.5f; // �Ŵ����ʱ��
    public Vector3 targetScale = new Vector3(1.2f, 1.2f, 1f); // Ŀ��Ŵ����
    void Start()
    {
        targetScale *= transform.localScale.x;
        // ͼƬ���ֺ���ԭ�ض������𽥱�Ϊ͸��
        targetSprite.transform.DOShakePosition(shakeDuration, new Vector3(1, 1, 0), 20, 90, false, true);
        targetSprite.transform.DOScale(targetScale, scaleDuration).SetLoops(2, LoopType.Yoyo);
        targetSprite.DOFade(0, fadeDuration).OnComplete(() =>
        {
            // ��ȫ͸����ͼƬ��������
            Destroy(targetSprite.gameObject);
        });
    }
}