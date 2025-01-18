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

        BubbleShip bs = collision.collider.GetComponentInParent<BubbleShip>();
        Rigidbody2D rb = bs.GetComponent<Rigidbody2D>();
        if(bs != null)
        {
            Debug.LogError("111");
        }


        if (wallType == WallType.normal)
        {
            // 遍历所有碰撞点（即使只有一个碰撞点，也能正确处理）
            foreach (ContactPoint2D contact in collision.contacts)
            {
                Vector2 normal = contact.normal;

                // 根据法向量朝向改变物体的速度朝向
                bs.AddForceToShip(Vector2.Reflect(rb.velocity, normal));

                // 打印法向量
                Debug.Log("Collision normal: " + normal);
            }
        }
        else if (wallType == WallType.bounce)
        {
            bs.AddForceToShip(NormalVector.normalized * rb.velocity.magnitude * bounceFactor);
        }

        else if(wallType == WallType.spike)
        {
            //todo: 戳爆最靠近的气泡
        }
        else if (wallType == WallType.corner)
        {
            // 遍历所有碰撞点（即使只有一个碰撞点，也能正确处理）
            foreach (ContactPoint2D contact in collision.contacts)
            {
                Vector2 normal = contact.normal;

                // 根据法向量朝向改变物体的速度朝向
                rb.velocity = Vector2.Reflect(rb.velocity, normal);

                // 打印法向量
                Debug.Log("Collision normal: " + normal);
            }
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
    checkPoint,
    spike,
    corner
}


