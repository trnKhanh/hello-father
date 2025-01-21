using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameDataManager : MonoBehaviour
{
    public static GameDataManager Instance { get; private set; }

    string saveFileName = "save";

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        Load();
        SceneManager.sceneLoaded += SceneManager_sceneLoaded;
    }

    private void SceneManager_sceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        Load();
    }

    public void Save()
    {
        Debug.Log("Saved");
        IGameData[] gameDatas = FindObjectsOfType<MonoBehaviour>(true).OfType<IGameData>().ToArray();
        string savePath = Path.Join(Application.persistentDataPath, saveFileName);
        Directory.CreateDirectory(savePath);

        foreach (IGameData gameData in gameDatas)
        {
            gameData.Save(savePath);
        }
    }

    public void Load()
    {
        Debug.Log("Loaded");
        IGameData[] gameDatas = FindObjectsOfType<MonoBehaviour>(true).OfType<IGameData>().ToArray();
        string savePath = Path.Join(Application.persistentDataPath, saveFileName);
        Directory.CreateDirectory(savePath);

        foreach (IGameData gameData in gameDatas)
        {
            gameData.Load(savePath);
        }
    }


    public bool HasSaveFile()
    {
        string savePath = Path.Join(Application.persistentDataPath, saveFileName);

        string sceneSaveFile = Path.Join(savePath, "scene.json");

        return File.Exists(sceneSaveFile);
    }

    public void DeleteSaveFile()
    {
        string savePath = Path.Join(Application.persistentDataPath, saveFileName);
        DirectoryInfo directoryInfo = new DirectoryInfo(savePath);
        if (directoryInfo.Exists)
            directoryInfo.Delete(true);
    }
}
