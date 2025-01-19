using System.Collections.Generic;
using UnityEngine;

public class LineDrawer : MonoBehaviour
{
    [SerializeField] private GameObject dotPrefab;
    [SerializeField] private List<GameObject> dots = new();
    [SerializeField] private float interval = 0.1f; // 可以根据需求调整点与点之间的间隔

    public Vector2 pos0;
    public Vector2 pos1;

    private void Update()
    {
        // 确保 LineRenderer 只在有节点的情况下更新线条
        if (dotPrefab != null)
        {
            DrawLine();
        }
    }

    private void DrawLine()
    {
        // 清除之前的点
        foreach (var dot in dots)
        {
            Destroy(dot);
        }
        dots.Clear();

        // 计算线段的方向和长度
        Vector2 direction = (pos1 - pos0).normalized;
        float distance = Vector2.Distance(pos0, pos1);
        int numDots = Mathf.CeilToInt(distance / interval); // 根据间隔获取所需点的数量

        // 创建点并绘制线段
        for (int i = 0; i <= numDots; i++)
        {
            Vector2 position = pos0 + direction * i * interval;
            GameObject dot = Instantiate(dotPrefab, position, Quaternion.identity);
            dot.transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 90);
            dots.Add(dot);
        }
    }
}