using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "BubbleData", menuName = "SO/BubbleData")]
public class BubbleData : SerializedScriptableObject
{
    public ExplodeEffect ExplodeEffect;
    public Sprite sprite;
    public Color color;
}

public abstract class ExplodeEffect
{
    public abstract void Effect(BubbleShip ship, Vector2 effectPos, float amount);
}

public class PushEffect : ExplodeEffect
{
    public float Factor;
    public override void Effect(BubbleShip ship, Vector2 effectPos, float amount)
    {
        var direction = (Vector2)ship.transform.position - effectPos;
        ship.AddForceToShip(direction.normalized * amount * ship.pushForceFactor * Factor);
    }
}

public class RotateEffect : ExplodeEffect
{
    public override void Effect(BubbleShip ship, Vector2 effectPos, float amount)
    {
        ship.RotateShip();
    }
}

public class GunEffect : ExplodeEffect
{
    public Bullet BulletPrefab;
    public override void Effect(BubbleShip ship, Vector2 effectPos, float amount)
    {
        var direction = (Vector2)ship.transform.position - effectPos;
        var bullet = GameObject.Instantiate(BulletPrefab, effectPos, Quaternion.identity);
        bullet.Init(-direction.normalized * 20);
    }
}

public class FrictionEffect : ExplodeEffect
{
    public override void Effect(BubbleShip ship, Vector2 effectPos, float amount)
    {
    }
}