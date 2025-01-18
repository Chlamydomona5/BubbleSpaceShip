using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class ComposeController : Singleton<ComposeController>
{
    [SerializeField] private float crabSpeed = 1f;
    [SerializeField] private float consumeLiquidSpeed = 1f;
    [SerializeField] private float minimumLiquid = 0.1f;
    [SerializeField] private float blowSpeed = 1f;
    [SerializeField] private float minimumBlowVolume = 0.1f;
    [SerializeField] private float bubbleInitialYOffset = 1f;

    [SerializeField] private GameObject crab;
    [SerializeField] private GameObject craw;
    [SerializeField] private LineRenderer lineRenderer;

    [SerializeField] private GeneratedBubble bubblePrefab;
    [SerializeField] private BubbleShip bubbleShip;
    [SerializeField, ReadOnly] private GeneratedBubble currentInitingBubble; 
    public LiquidBottle currentTouchingLiquidBottle;

    [SerializeField] private List<GameObject> hideOnStart;
    [SerializeField] private List<GameObject> showOnStart;
    
    [SerializeField] private List<LiquidBottle> liquidBottles;
    [SerializeField] private List<ComposeBubbleBase> allBubbles;

    [SerializeField] private List<LiquidSet> levelSets;
    [SerializeField] private Transform originPosition;

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
        // GenerateBubble
        if (Input.GetKey(KeyCode.Space))
        {
            // If there is no bubble initing, and there is a liquid bottle touching
            if (!currentInitingBubble && currentTouchingLiquidBottle)
            {
                TryGenerateBubbleAt(currentTouchingLiquidBottle.Data, currentTouchingLiquidBottle.transform.position);
            }
            // If there is already a bubble initing
            if (currentInitingBubble)
            {
                BlowCurrentBubble();
            }
        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            if (currentInitingBubble)
            {
                ReleaseCurrentBubble();
            }
        }
        else
        {
            // Crab Move
            if(Input.GetKey(KeyCode.A)) crab.transform.position += Vector3.left * (crabSpeed * Time.deltaTime);
            if(Input.GetKey(KeyCode.D)) crab.transform.position += Vector3.right * (crabSpeed * Time.deltaTime);
        }
        
        if(Input.GetKeyDown(KeyCode.R))
        {
            ResetGame();
        }
    }
    
    public void TryGenerateBubbleAt(BubbleData data, Vector2 pos)
    {
        if (currentTouchingLiquidBottle.TryConsumeLiquid(minimumLiquid))
        {
            var bubble = Instantiate(bubblePrefab, pos + Vector2.up * bubbleInitialYOffset, Quaternion.identity);
            bubble.Init(data);
            currentInitingBubble = bubble;
            currentInitingBubble.Blow(minimumBlowVolume);
            
            allBubbles.Add(bubble);
        }
    }

    public bool BlowCurrentBubble()
    {
        var delta = Time.deltaTime;
        if (currentTouchingLiquidBottle.TryConsumeLiquid(delta * consumeLiquidSpeed))
        {
            currentInitingBubble.Blow(delta * blowSpeed);
            return true;
        }
        return false;
    }
    
    private void ReleaseCurrentBubble()
    {
        currentInitingBubble.Release();
        currentInitingBubble = null;
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
        liquidBottles.ForEach(x => x.ResetBottle());
        for (int i = 0; i < allBubbles.Count; i++)
        {
            if(allBubbles[i] && allBubbles[i].gameObject)
                Destroy(allBubbles[i].gameObject);
        }
        allBubbles.Clear();
    }

    public void ReadLiquidSet(int level)
    {
        liquidBottles.ForEach(x => x.gameObject.SetActive(false));
        
        var set = levelSets[level];
        for (int i = 0; i < set.LiquidBottles.Count; i++)
        {
            liquidBottles[i].gameObject.SetActive(true);
            liquidBottles[i].Read(set.LiquidBottles[i]);
        }
    }

    public void ResetGame()
    {
        ReadLiquidSet(GameManager.Instance.LevelStage);
        ResetCompose();
        bubbleShip.transform.position = originPosition.position;
        bubbleShip.ActualMove(false);
        hideOnStart.ForEach(go => go.SetActive(true));
        showOnStart.ForEach(go => go.SetActive(false));
    }
}