using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneStateManager : MonoBehaviour
{
    public static SceneStateManager Instance { get; private set; }

    [Header("Scene settings")]
    public SceneLoadingManager.SceneType nextScene;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void LoadNextScene()
    {
        SceneLoadingManager.Instance.LoadScene(nextScene);
    }

    public void LoadMenu()
    {
        SceneLoadingManager.Instance.LoadMenuScene();
    }

    public void Restart()
    {
        SceneLoadingManager.Instance.ReloadScene();
    }
}
