using System.Collections.Generic;
using DG.Tweening;
using Sirenix.Serialization;
using UnityEngine;

public class ComposeController : Singleton<ComposeController>
{
    [SerializeField] private float crabSpeed = 1f;

    [SerializeField] private GameObject crab;
    [SerializeField] private GameObject craw;
    [SerializeField] private Transform crawOpen;
    [SerializeField] private Transform crawClose;
    [SerializeField] private LineRenderer lineRenderer;

    [SerializeField] private GeneratedBubble bubblePrefab;
    [SerializeField] private BubbleShip bubbleShip;

    [SerializeField] private List<GameObject> hideOnStart;
    [SerializeField] private List<GameObject> showOnStart;

    [SerializeField] private List<ComposeBubbleBase> allBubbles;
    [SerializeField] private Transform shipOrigin;
    [SerializeField] private Transform bubbleOrigin;

    [OdinSerialize] private List<(BubbleData data, float size)> _bubbles;

    [SerializeField] private Camera fixedCamera;
    [SerializeField] private Camera fixedSmallCamera;
    [SerializeField] private Camera fixedBigCamera;
    private bool _cameraZoomed;

    private void Start()
    {
        GenerateAllBubbles();

        Cursor.visible = false;
    }

    private void GenerateAllBubbles()
    {
        foreach (var bubble in _bubbles)
        {
            var instance = Instantiate(bubblePrefab, bubbleOrigin.position + (Vector3)Random.insideUnitCircle, Quaternion.identity);
            instance.Init(bubble.data, bubble.size);
            allBubbles.Add(instance);
        }
    }

    private void Update()
    {
        var mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPosition = new Vector3(mouseWorldPosition.x, mouseWorldPosition.y, 0);

        // Craw Move
        craw.transform.position = mouseWorldPosition;
        // Adjust Craw Angle align to crab
        var direction = (crab.transform.position - craw.transform.position).normalized;
        var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        craw.transform.rotation = Quaternion.Euler(0, 0, angle + 90);
        // Adjust Line
        lineRenderer.SetPosition(0, crab.transform.position + Vector3.right / 2f);
        lineRenderer.SetPosition(1, craw.transform.position);

        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetGame();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            CameraZoom();
        }

        if (Input.GetMouseButtonDown(0))
        {
            crawOpen.gameObject.SetActive(false);
            crawClose.gameObject.SetActive(true);
        }
        
        if (Input.GetMouseButtonUp(0))
        {
            crawOpen.gameObject.SetActive(true);
            crawClose.gameObject.SetActive(false);
        }
        
    }

    private void CameraZoom()
    {
        if (!_cameraZoomed)
        {
            fixedCamera.DOOrthoSize(fixedBigCamera.orthographicSize, 1f);
            fixedCamera.transform.DOMove(fixedBigCamera.transform.position, 1f);
            _cameraZoomed = true;
        }
        else
        {
            fixedCamera.DOOrthoSize(fixedSmallCamera.orthographicSize, 1f);
            fixedCamera.transform.DOMove(fixedSmallCamera.transform.position, 1f);
            _cameraZoomed = false;
        }
    }

    public void StartActualMove()
    {
        Destroy(crab.GetComponent<Rigidbody2D>());
        Destroy(crab.GetComponent<Collider2D>());
        bubbleShip.Stop();
        crab.transform.DOJump(bubbleShip.transform.position, 1f, 1, 1f).OnComplete((() =>
        {
            bubbleShip.ActualMove(true);
            hideOnStart.ForEach(go => go.SetActive(false));
            showOnStart.ForEach(go => go.SetActive(true));
            crab.transform.SetParent(bubbleShip.transform);
        }));
    }

    public void ResetCompose()
    {
        bubbleShip.ResetShip();
        for (int i = 0; i < allBubbles.Count; i++)
        {
            if (allBubbles[i] && allBubbles[i].gameObject)
                Destroy(allBubbles[i].gameObject);
        }

        allBubbles.Clear();
        GenerateAllBubbles();
    }

    public void ResetGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }
}