using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private void Awake()
    {
        Instance = this;
    }
    
    public void LoadNextLevel()
    {
        if (UnityEngine.SceneManagement.SceneManager.sceneCountInBuildSettings ==
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex + 1)
        {
            Debug.Log("Win");
        }
        else 
            UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex + 1);
    }
}
