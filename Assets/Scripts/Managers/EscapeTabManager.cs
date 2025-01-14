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
            Debug.Log("Pause");
            if (!paused)
                Pause();
            else
                Resume();
        }
    }

    public void Pause()
    {
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
        quitButton.onClick.AddListener(quitButton_onClick);
    }

    void UnsubribeToControlEvents()
    {
        resumeButton.onClick.RemoveListener(resumeButton_onClick);
        quitButton.onClick.RemoveListener(quitButton_onClick);
    }

    void resumeButton_onClick()
    {
        Resume();
    }

    void quitButton_onClick()
    {
        SceneStateManager.Instance.LoadMenu();
    }

    void Show()
    {
        canvas.gameObject.SetActive(true);
        Time.timeScale = 0;
        ControlManager.Instance.UnlockCursor();
    }

    void Hide()
    {
        canvas.gameObject.SetActive(false);
        Time.timeScale = 1;
        ControlManager.Instance.LockCursor();
    }
}
