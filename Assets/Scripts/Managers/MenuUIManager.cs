using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuUIManager : MonoBehaviour
{
    [Header("References")]
    public Button newGameButton;
    public Button quitButton;

    string k_transition = "Transition";

    Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void OnEnable()
    {
        SubribeToButtonEvents();
    }

    void OnDisable()
    {
        UnsubribeToButtonEvents();
    }

    private void Update()
    {
        if (Input.anyKeyDown)
        {
            animator.SetTrigger(k_transition);
        }
    }

    void SubribeToButtonEvents()
    {
        UnsubribeToButtonEvents();

        newGameButton.onClick.AddListener(newGameButton_onClick);
        quitButton.onClick.AddListener(quitButton_onClick);
    }

    void UnsubribeToButtonEvents()
    {
        newGameButton.onClick.RemoveListener(newGameButton_onClick);
        quitButton.onClick.RemoveListener(quitButton_onClick);
    }

    void newGameButton_onClick()
    {
        SceneStateManager.Instance.LoadNextScene();
    }

    void quitButton_onClick()
    {
        Application.Quit();
    }
}
