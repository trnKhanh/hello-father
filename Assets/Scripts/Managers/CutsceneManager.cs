using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class CutsceneManager : MonoBehaviour, IGameData
{
    public static CutsceneManager Instance { get; private set; }

    public class CutsceneData
    {
        public int cutsceneId = 0;
    }

    public CutsceneData cutsceneData = new CutsceneData();

    [Header("References")]
    public GameObject[] gameplayObjects;

    [Header("Cutscenes")]
    public GameObject[] cutscenes;

    GameObject currentCutscene;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void LoadCutscene(int id)
    {
        if (id >= cutscenes.Length)
            return;

        foreach (GameObject cutscene in cutscenes)
        {
            cutscene.SetActive(false);
        }
        foreach (GameObject gameObject in gameplayObjects)
        {
            gameObject.SetActive(false);
        }

        currentCutscene = cutscenes[id];
        currentCutscene.SetActive(true);
    }

    public void LoadNextCutscene()
    {
        AudioManager.Instance.MuteBackGround();
        LoadCutscene(cutsceneData.cutsceneId++);
    }

    public void OnCutseneStopped()
    {
        foreach (GameObject gameObject in gameplayObjects)
        {
            gameObject.SetActive(true);
        }
        currentCutscene.SetActive(false);

        ControlManager.Instance.ResumeGame();
        ControlManager.Instance.LockCursor();

        GameDataManager.Instance.Save();
        AudioManager.Instance.UnMuteBackGround();

        if (cutsceneData.cutsceneId >= cutscenes.Length)
        {
            SceneStateManager.Instance.LoadNextScene();
        }
    }

    public void Save(string root)
    {
        string savePath = Path.Join(root, "cutscene.json");
        File.WriteAllText(savePath, JsonUtility.ToJson(cutsceneData));
    }

    public void Load(string root)
    {
        try
        {
            string savePath = Path.Join(root, "cutscene.json");
            cutsceneData = JsonUtility.FromJson<CutsceneData>(File.ReadAllText(savePath));
        }
        catch (Exception e)
        {
            cutsceneData.cutsceneId = 0;
            Debug.LogWarning(e);
        }
        Debug.Log("Load Cutscene");

        if (cutsceneData.cutsceneId == 0)
        {
            LoadNextCutscene();
        } else
        {
            foreach (GameObject gameObject in gameplayObjects)
            {
                gameObject.SetActive(true);
            }
        }
    }
}
