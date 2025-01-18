using UnityEngine;

public class SceneManager : Singleton<SceneManager>
{
    public override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
    }

    public void LoadToLevel(string levelName)
    {
        ComposeController.Instance.StartActualMove();
        UnityEngine.SceneManagement.SceneManager.LoadScene(levelName);
    }
}