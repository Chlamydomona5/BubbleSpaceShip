using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LevelWallBase : MonoBehaviour
{
    public WallType wallType;
    public Vector2 NormalVector;
    public float bounceFactor = 1f;
    public int stage;
    private bool disabled = false;
    public bool disableRender = true;
    private void Awake()
    {
        /*if(disableRender)
            GetComponent<SpriteRenderer>().enabled = false;*/
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.collider.gameObject.layer != LayerMask.NameToLayer("Bubble"))
        {
            return;
        }

        BubbleShip bs = collision.collider.GetComponentInParent<BubbleShip>();
        if(!bs) return;

        if (wallType == WallType.normal || wallType == WallType.bounce)
        {
            GeneratedBubble gb = collision.collider.gameObject.GetComponent<GeneratedBubble>();
            if (gb && gb.Data && gb.Data.ExplodeEffect is FrictionEffect)
            {

            }
            else
            {
                // 遍历所有碰撞点（即使只有一个碰撞点，也能正确处理）
                foreach (ContactPoint2D contact in collision.contacts)
                {
                    Vector2 normal = -contact.normal;
                    normal = FindClosestDirection(normal);
                    // 根据法向量朝向改变物体的速度朝向
                    normal = Vector2.Reflect(bs.previousVelocity, normal) * (1f / Time.fixedDeltaTime);
                    bs.AddForceToShip(normal);
                }
            }
        }
        else if (wallType == WallType.spike)
        {
            //todo: 戳爆最靠近的气泡,
            collision.collider.gameObject.GetComponent<GeneratedBubble>().Explode(true);
        }
        else if (wallType == WallType.checkPoint)
        {
            GameManager.Instance.LoadNextLevel();
        }

    }
    public static Vector2 FindClosestDirection(Vector2 inputNormal)
    {
        // 定义八个方向向量
        Vector2[] directions = new Vector2[]
        {
            Vector2.up,       // 上
            Vector2.down,     // 下
            Vector2.left,     // 左
            Vector2.right,    // 右
            new Vector2(-1, 1).normalized,   // 左上
            new Vector2(-1, -1).normalized,  // 左下
            new Vector2(1, 1).normalized,    // 右上
            new Vector2(1, -1).normalized    // 右下
        };

        Vector2 closestDir = directions[0];
        float smallestAngle = Vector2.Angle(inputNormal, closestDir);

        foreach (Vector2 dir in directions)
        {
            float angle = Vector2.Angle(inputNormal, dir);
            if (angle < smallestAngle)
            {
                smallestAngle = angle;
                closestDir = dir;
            }
        }

        return closestDir;
    }
}
public enum WallType
{
    normal,
    bounce,
    checkPoint,
    spike,
    corner
}




