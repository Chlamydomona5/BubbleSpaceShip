using UnityEngine;

public class CoreBubble : ComposeBubbleBase
{
    public override void Explode()
    {
        ComposeController.Instance.ResetGame();
    }
}