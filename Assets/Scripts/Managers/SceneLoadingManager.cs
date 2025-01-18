using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoadingManager : MonoBehaviour, IGameData
{
    public static SceneLoadingManager Instance { get; private set; }

    public class SceneStateData
    {
        public string currentScene;
    }

    public SceneStateData sceneStateData = new SceneStateData();

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
        ControlManager.Instance.ResumeGame();
        string sceneName = GetSceneName(sceneType);

        if (sceneName != null)
        {
            LoadingScreen.Instance.StartLoading(sceneName);
        }
    }

    public void LoadSavedScene()
    {
        Debug.Log(sceneStateData.currentScene);
        LoadingScreen.Instance.StartLoading(sceneStateData.currentScene);
    }

    public void LoadMenuScene()
    {
        LoadScene(SceneType.Menu);
    }

    public string GetCurrentScene()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        return currentSceneName;
    }

    public void ReloadScene()
    {
        ControlManager.Instance.ResumeGame();
        string currentSceneName = SceneManager.GetActiveScene().name;
        LoadingScreen.Instance.StartLoading(currentSceneName);
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


    public void Save(string root)
    {
        string savePath = Path.Join(root, "scene.json");
        sceneStateData.currentScene = GetCurrentScene();
        File.WriteAllText(savePath, JsonUtility.ToJson(sceneStateData));
    }

    public void Load(string root)
    {
        try
        {
            string savePath = Path.Join(root, "scene.json");
            sceneStateData = JsonUtility.FromJson<SceneStateData>(File.ReadAllText(savePath));
        }
        catch (Exception e)
        {
            Debug.LogWarning(e);
        }
    }
}
