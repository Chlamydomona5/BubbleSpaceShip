using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int LevelStage = 0;
    public static GameManager Instance;
    private void Awake()
    {
        Instance = this;
    }


    public void updateLevelCheckpoint(int checkPointStage)
    {
        if(checkPointStage > LevelStage)
            LevelStage = checkPointStage;

        //todo 更新可用的材料
    }
}
