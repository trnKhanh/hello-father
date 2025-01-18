using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HintManager : MonoBehaviour
{
    static public HintManager Instance { get; private set; }

    [Header("Hint Settings")]
    public float timeout;
    float timeleft;

    [Header("References")]
    public RectTransform hintPanelUI;
    public TMP_Text hintTextUI;
    public TMP_Text hintKeyUI;

    public FatherMovement fatherMovement;

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
        timeleft -= Time.deltaTime;
        if (timeleft <= 0)
        {
            HideHintUI();
        }
    }

    public void ShowHint(KeyCode key, string text)
    {
        hintKeyUI.text = key.ToString();
        hintTextUI.text = text;

        timeleft = timeout;
        ShowHintUI();
    }

    void ShowHintUI()
    {
        hintPanelUI.gameObject.SetActive(true);
    }

    void HideHintUI()
    {
        hintPanelUI.gameObject.SetActive(false);
    }
}

