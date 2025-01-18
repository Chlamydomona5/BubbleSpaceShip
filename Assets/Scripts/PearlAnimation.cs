using DG.Tweening;
using UnityEngine;

public class PearlAnimation : MonoBehaviour
{
    private void Start()
    {
        transform.DOMoveY(transform.position.y + 0.2f, 1f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);
        transform.DORotate(new Vector3(0, 360, 0), 3f, RotateMode.FastBeyond360).SetLoops(-1, LoopType.Restart).SetEase(Ease.Linear);
    }
}