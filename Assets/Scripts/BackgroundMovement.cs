using UnityEngine;

public class BackgroundMovement : MonoBehaviour
{
    private Vector3 startPos;
    private Vector3 camStartPos;
    public Camera cam;
    public float moveRate = 0.8f; // ±³¾°ÒÆ¶¯ËÙ¶È£¬80%

    void Start()
    {
        startPos = transform.position;
        camStartPos = cam.transform.position;
    }

    void Update()
    {
        Vector3 camOffset = cam.transform.position - camStartPos;
        transform.position = startPos + camOffset * moveRate;
    }
}
