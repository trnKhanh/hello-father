using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoadingManager : MonoBehaviour
{
    public static SceneLoadingManager Instance { get; private set; }

    public enum SceneType
    {
        Menu,
        Map1,
        Map2,
    }

    [Serializable]
    public struct SceneData
    {
        public SceneType sceneType;
        public string sceneName;
    }

    [Header("Global scenes")]
    public SceneData[] sceneDatas;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void LoadScene(SceneType sceneType)
    {
        string sceneName = GetSceneName(sceneType);

        if (sceneName != null)
        {
            SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
        }
    }

    public void LoadMenuScene()
    {
        LoadScene(SceneType.Menu);
    }

    public void ReloadScene()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
    }

    private string GetSceneName(SceneType type)
    {
        foreach (SceneData data in sceneDatas)
        {
            if (data.sceneType == type)
                return data.sceneName;
        }
        return null;
    }
}
