using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "LiquidSet", menuName = "SO/LiquidSet")]
public class LiquidSet : SerializedScriptableObject
{
    public List<(BubbleData, float)> LiquidBottles;
}