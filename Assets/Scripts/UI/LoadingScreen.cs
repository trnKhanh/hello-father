using System.Collections;
using System.Collections.Generic;
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

    public void LoadLevelBtn(string levelToLoad)
    {
        loadingScreenPanel.gameObject.SetActive(true);

        StartCoroutine(LoadLevelAsync(levelToLoad));
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
