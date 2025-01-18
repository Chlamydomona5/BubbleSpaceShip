using System;
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

    [SerializeField] private ComposeBubbleBase bubblePrefab;
    [SerializeField, ReadOnly] private ComposeBubbleBase currentInitingBubble; 
    public LiquidBottle currentTouchingLiquidBottle;
    
    private void Update()
    {
        var mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPosition = new Vector3(mouseWorldPosition.x, mouseWorldPosition.y, 0);

        // Craw Move
        craw.transform.position = mouseWorldPosition;
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
            if(Input.GetKey(KeyCode.A)) crab.transform.position += Vector3.left * crabSpeed * Time.deltaTime;
            if(Input.GetKey(KeyCode.D)) crab.transform.position += Vector3.right * crabSpeed * Time.deltaTime;
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
}