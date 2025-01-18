using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelWallBase : MonoBehaviour
{
    public WallType wallType;
    public Vector2 NormalVector;
    public float bounceFactor = 1f;
    public int stage;
    private bool disabled = false;


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.layer != LayerMask.NameToLayer("Bubble"))
        {
            return;
        }
        BubbleShip bs = collision.gameObject.GetComponent<BubbleShip>();
        if(bs != null)
        {
            Debug.LogError("111");
        }


        if (wallType == WallType.normal)
        {
            bs.AddForceToShip(NormalVector.normalized * bounceFactor);
        }
        else if (wallType == WallType.bounce)
        {
            bs.AddForceToShip(NormalVector.normalized * bounceFactor);
        }
        else if (wallType == WallType.checkPoint) 
        {
            GameManager.Instance.updateLevelCheckpoint(stage);
        }
    }
}
public enum WallType
{
    normal,
    bounce,
    checkPoint
}
