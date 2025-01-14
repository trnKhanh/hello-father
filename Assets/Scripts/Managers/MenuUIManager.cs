using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuUIManager : MonoBehaviour
{
    [Header("References")]
    public Button playButton;
    public Button quitButton;

    void OnEnable()
    {
        SubribeToButtonEvents();
    }

    void OnDisable()
    {
        UnsubribeToButtonEvents();
    }

    void SubribeToButtonEvents()
    {
        UnsubribeToButtonEvents();

        playButton.onClick.AddListener(playButton_onClick);
        quitButton.onClick.AddListener(quitButton_onClick);
    }

    void UnsubribeToButtonEvents()
    {
        playButton.onClick.RemoveListener(playButton_onClick);
        quitButton.onClick.RemoveListener(quitButton_onClick);
    }

    void playButton_onClick()
    {
        SceneStateManager.Instance.LoadNextScene();
    }

    void quitButton_onClick()
    {
        Application.Quit();
    }
}
