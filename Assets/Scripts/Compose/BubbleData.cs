using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "BubbleData", menuName = "SO/BubbleData")]
public class BubbleData : SerializedScriptableObject
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
        var direction = (Vector2)ship.transform.position - effectPos;
        ship.AddForceToShip(direction.normalized * amount);
    }
}

public class RotateEffect : ExplodeEffect
{
    public override void Effect(BubbleShip ship, Vector2 effectPos, float amount)
    {
        ship.RotateShip();
    }
}