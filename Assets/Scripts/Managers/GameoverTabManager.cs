using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameoverTabManager : MonoBehaviour
{
    static public GameoverTabManager Instance { get; private set; }

    [Header("Gameover Tab References")]
    public Canvas canvas;

    [Header("Control References")]
    public Button restartButton;
    public Button quitButton;

    Animator animator;

    string k_gamevover_anim = "Gameover";

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        animator = canvas.gameObject.GetComponent<Animator>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            GameOver();
        }
    }

    public void GameOver()
    {
        Show();
        SubribeToControlEvents();
    }

    void SubribeToControlEvents()
    {
        UnsubribeToControlEvents();

        restartButton.onClick.AddListener(resumeButton_onClick);
        quitButton.onClick.AddListener(quitButton_onClick);
    }

    void UnsubribeToControlEvents()
    {
        restartButton.onClick.RemoveListener(resumeButton_onClick);
        quitButton.onClick.RemoveListener(quitButton_onClick);
    }

    void resumeButton_onClick()
    {
        SceneStateManager.Instance.Restart();
    }

    void quitButton_onClick()
    {
        SceneStateManager.Instance.LoadMenu();
    }

    void Show()
    {
        AudioManager.Instance.PlayBackgroundMusic(AudioManager.BackgroundState.Gameover);
        ControlManager.Instance.OpenTab(canvas, false);
    }

    void Hide()
    {
        ControlManager.Instance.CloseTab(canvas);
    }
}
