using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelWallBase : MonoBehaviour
{
    public WallType wallType;
}
public enum WallType
{
    normal,
    bounce
}
