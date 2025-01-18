using UnityEngine;

public class CoreBubble : ComposeBubbleBase
{
    public override void Explode(bool checkConnection)
    {
        ComposeController.Instance.ResetGame();
    }
}