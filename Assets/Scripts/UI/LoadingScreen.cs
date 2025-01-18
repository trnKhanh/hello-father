using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class LoadingScreen : MonoBehaviour
{

    static public LoadingScreen Instance { get; private set; }

    [SerializeField] private RectTransform loadingScreenPanel;
    [SerializeField] private Slider loadingScreenSlider;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(this);
    }

    private void Start()
    {
        SceneManager.sceneLoaded += SceneManager_sceneLoaded;
    }

    private void SceneManager_sceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        Hide();
    }

    public void StartLoading(string levelName)
    {
        if (string.IsNullOrEmpty(levelName))
        {
            return;
        }
        StartCoroutine(LoadLevelAsync(levelName));
        Show();
    }

    public void Show()
    {
        if (Instance != null)
        {
            loadingScreenPanel.gameObject.SetActive(true);

        }
    }

    public void Hide()
    {
        if (Instance != null)
        {
            loadingScreenPanel.gameObject.SetActive(false);
        }
    }

    IEnumerator LoadLevelAsync(string levelToLoad)
    {
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(levelToLoad);
        while (!loadOperation.isDone)
        {
            float progressValue = Mathf.Clamp01(loadOperation.progress / 0.9f);
            loadingScreenSlider.value = progressValue;
            yield return null;
        }
    }
}
