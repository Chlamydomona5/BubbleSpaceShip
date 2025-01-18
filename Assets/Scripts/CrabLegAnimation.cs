using System;
using DG.Tweening;
using UnityEngine;

public class CrabLegAnimation : MonoBehaviour
{
    private void Start()
    {
        // Endless loop
        transform.DORotate(new Vector3(0, 0, 15), 1f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);
    }
}