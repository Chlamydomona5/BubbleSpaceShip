using System;
using Sirenix.OdinInspector;
using UnityEngine;

public class LiquidBottle : MonoBehaviour
{
    [SerializeField] private BubbleData data;
    public BubbleData Data => data;
    [SerializeField] private float maxLiquidVolume;

    [SerializeField, ReadOnly] private float _currentLiquidVolume;

    private void Awake()
    {
        _currentLiquidVolume = maxLiquidVolume;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Crab")) ComposeController.Instance.currentTouchingLiquidBottle = this;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if(other.CompareTag("Crab")) ComposeController.Instance.currentTouchingLiquidBottle = null;
    }
    
    public bool TryConsumeLiquid(float consumeVolume)
    {
        if (_currentLiquidVolume >= consumeVolume)
        {
            _currentLiquidVolume -= consumeVolume;
            return true;
        }
        return false;
    }
}