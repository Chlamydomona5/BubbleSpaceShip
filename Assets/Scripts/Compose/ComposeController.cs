using System.Collections.Generic;
using Sirenix.Serialization;
using UnityEngine;

public class ComposeController : Singleton<ComposeController>
{
    [SerializeField] private float crabSpeed = 1f;

    [SerializeField] private GameObject crab;
    [SerializeField] private GameObject craw;
    [SerializeField] private LineRenderer lineRenderer;

    [SerializeField] private GeneratedBubble bubblePrefab;
    [SerializeField] private BubbleShip bubbleShip;

    [SerializeField] private List<GameObject> hideOnStart;
    [SerializeField] private List<GameObject> showOnStart;
    
    [SerializeField] private List<ComposeBubbleBase> allBubbles;
    [SerializeField] private Transform shipOrigin;
    [SerializeField] private Transform bubbleOrigin;

    [OdinSerialize] private List<List<(BubbleData data, float size)>> _bubbles;

    private void Start()
    {
        ResetGame();
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
        lineRenderer.SetPosition(0, crab.transform.position);
        lineRenderer.SetPosition(1, craw.transform.position);
        
        if(Input.GetKeyDown(KeyCode.R))
        {
            ResetGame();
        }
    }

    public void StartActualMove()
    {
        bubbleShip.ActualMove(true);
        hideOnStart.ForEach(go => go.SetActive(false));
        showOnStart.ForEach(go => go.SetActive(true));
    }

    public void ResetCompose()
    {
        bubbleShip.ResetShip();
        for (int i = 0; i < allBubbles.Count; i++)
        {
            if(allBubbles[i] && allBubbles[i].gameObject)
                Destroy(allBubbles[i].gameObject);
        }
        allBubbles.Clear();
    }

    public void ResetGame()
    {
        ResetCompose();
        bubbleShip.transform.position = shipOrigin.position;
        bubbleShip.ActualMove(false);
        hideOnStart.ForEach(go => go.SetActive(true));
        showOnStart.ForEach(go => go.SetActive(false));

        foreach (var level in _bubbles)
        {
            foreach (var bubble in level)
            {
                var instance = Instantiate(bubblePrefab, bubbleOrigin);
                instance.Init(bubble.data, bubble.size);
            }
        }
    }
}