using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

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
        return Directory.Exists(savePath);
    }

    public void DeleteSaveFile()
    {
        string savePath = Path.Join(Application.persistentDataPath, saveFileName);
        DirectoryInfo directoryInfo = new DirectoryInfo(savePath);
        if (directoryInfo.Exists)
            directoryInfo.Delete(true);
    }
}
