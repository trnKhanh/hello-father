using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuUIManager : MonoBehaviour
{
    [Header("References")]
    public Button continueButton;
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
        if (GameDataManager.Instance.HasSaveFile())
        {
            continueButton.gameObject.SetActive(true);
        }
        else
        {
            continueButton.gameObject.SetActive(false);
        }

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

    public void SubribeToButtonEvents()
    {
        UnsubribeToButtonEvents();

        continueButton.onClick.AddListener(continueButton_onClick);
        newGameButton.onClick.AddListener(newGameButton_onClick);
        quitButton.onClick.AddListener(quitButton_onClick);
    }

    void UnsubribeToButtonEvents()
    {
        continueButton.onClick.RemoveListener(continueButton_onClick);
        newGameButton.onClick.RemoveListener(newGameButton_onClick);
        quitButton.onClick.RemoveListener(quitButton_onClick);
    }

    void continueButton_onClick()
    {
        SceneStateManager.Instance.LoadSavedScene();
    }

    void newGameButton_onClick()
    {
        GameDataManager.Instance.DeleteSaveFile();
        SceneStateManager.Instance.LoadNextScene();
    }

    void quitButton_onClick()
    {
        Application.Quit();
    }
}
