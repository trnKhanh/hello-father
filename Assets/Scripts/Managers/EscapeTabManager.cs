using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EscapeTabManager : MonoBehaviour
{
    static public EscapeTabManager Instance { get; private set; }

    [Header("Escape Tab References")]
    public Canvas canvas;

    [Header("Control References")]
    public Button resumeButton;
    public Button saveGameButton;
    public Button loadGameButton;
    public Button quitButton;

    bool paused = false;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
            return;
        }
        Instance = this;
    }

    void Update()
    {
        if (InputManager.Instance.escape)
        {
            if (!paused)
                Pause();
            else
                Resume();
        }
    }

    public void Pause()
    {
        if (ControlManager.Instance.frameOpenedTabs != 0)
            return;

        paused = true;

        Show();
        SubribeToControlEvents();
    }

    public void Resume()
    {
        paused = false;

        UnsubribeToControlEvents();
        Hide();
    }

    void SubribeToControlEvents()
    {
        UnsubribeToControlEvents();

        resumeButton.onClick.AddListener(resumeButton_onClick);
        saveGameButton.onClick.AddListener(saveGameButton_onClick);
        loadGameButton.onClick.AddListener(loadGameButton_onClick);
        quitButton.onClick.AddListener(quitButton_onClick);
    }

    void UnsubribeToControlEvents()
    {
        resumeButton.onClick.RemoveListener(resumeButton_onClick);
        saveGameButton.onClick.RemoveListener(saveGameButton_onClick);
        loadGameButton.onClick.RemoveListener(loadGameButton_onClick);
        quitButton.onClick.RemoveListener(quitButton_onClick);
    }

    void resumeButton_onClick()
    {
        Resume();
    }

    void saveGameButton_onClick()
    {
        GameDataManager.Instance.Save();
    }

    void loadGameButton_onClick()
    {
        SceneStateManager.Instance.Restart();
    }


    void quitButton_onClick()
    {
        SceneStateManager.Instance.LoadMenu();
    }

    void Show()
    {
        ControlManager.Instance.OpenTab(canvas);
    }

    void Hide()
    {
        ControlManager.Instance.CloseTab(canvas);

    }
}
