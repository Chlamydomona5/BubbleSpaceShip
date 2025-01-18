using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private void Awake()
    {
        Instance = this;
    }
    
    public void LoadNextLevel()
    {
        if (UnityEngine.SceneManagement.SceneManager.sceneCount ==
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex + 1)
        {
            Debug.Log("Win");
        }
        else 
            UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex + 1);
    }
}
