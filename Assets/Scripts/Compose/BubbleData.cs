using UnityEngine;

[CreateAssetMenu(fileName = "BubbleData", menuName = "SO/BubbleData")]
public class BubbleData : ScriptableObject
{
    public ExplodeEffect ExplodeEffect;
}

public abstract class ExplodeEffect
{
    public abstract void Effect(BubbleShip ship, Vector2 effectPos, float amount);
}

public class PushEffect : ExplodeEffect
{
    public override void Effect(BubbleShip ship, Vector2 effectPos, float amount)
    {
        
    }
}

public class RotateEffect : ExplodeEffect
{
    public override void Effect(BubbleShip ship, Vector2 effectPos, float amount)
    {
        
    }
}