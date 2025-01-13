using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ReadingManager : MonoBehaviour
{
    static public ReadingManager Instance { get; private set; }

    [Header("Readable References")]
    public Canvas canvas;
    public PageController[] pagesObjects;
    PageController currentReading;

    [Header("Control References")]
    public Button prevButton;
    public Button nextButton;
    public Button closeButton;


    void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
            return;
        }
        Instance = this;
    }

    void OnEnable()
    {
        closeButton.onClick.AddListener(closeButton_onClick);
    }

    void OnDisable()
    {
        closeButton.onClick.RemoveListener(closeButton_onClick);
    }

    public void StartRead(PageController readableObject)
    {
        currentReading = readableObject;
        RefreshUI();
        UpdateControlUI();
        Show();
    }

    void RefreshUI()
    {
        foreach (PageController obj in pagesObjects)
        {
            obj.gameObject.SetActive(false);
        }

        if (currentReading != null)
            currentReading.gameObject.SetActive(true);
    }

    void UpdateControlUI()
    {
        prevButton.gameObject.SetActive(false);
        prevButton.onClick.RemoveListener(prevButton_onClick);

        nextButton.gameObject.SetActive(false);
        nextButton.onClick.RemoveListener(nextButton_onClick);

        if (currentReading != null)
        {
            if (currentReading.CanGoToPreviousPage())
            {
                prevButton.gameObject.SetActive(true);
                prevButton.onClick.AddListener(prevButton_onClick);
            }

            if (currentReading.CanGoToNextPage())
            {
                nextButton.gameObject.SetActive(true);
                nextButton.onClick.AddListener(nextButton_onClick);
            }

        }
    }

    void prevButton_onClick()
    {
        if (currentReading != null)
        {
            Debug.Log("prevButton_onClick");
            currentReading.PreviousPage();
            AudioManager.Instance.PlaySFX(AudioManager.SFXState.TurnPage);
            UpdateControlUI();
        }
    }

    void nextButton_onClick()
    {
        if (currentReading != null)
        {
            Debug.Log("nextButton_onClick");
            currentReading.NextPage();
            AudioManager.Instance.PlaySFX(AudioManager.SFXState.TurnPage);
            UpdateControlUI();
        }
    }

    void closeButton_onClick()
    {
        Hide();
    }

    void Show()
    {
        canvas.gameObject.SetActive(true);
        Time.timeScale = 0;
        CursorManager.Instance.UnlockCursor();
    }

    void Hide()
    {
        canvas.gameObject.SetActive(false);
        Time.timeScale = 1;
        CursorManager.Instance.LockCursor();
    }

}
