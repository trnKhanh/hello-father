using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlManager : MonoBehaviour
{
    static public ControlManager Instance { get; private set; }

    [Header("Start Settings")]
    public bool startLocking = true;
    public bool startPausing = false;

    public List<Canvas> openedTabs { get; private set; } = new List<Canvas>();
    public int frameOpenedTabs { get; private set; } = 0;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
            return;
        }
        Instance = this;
    }

    void Start()
    {
        if (startLocking)
            LockCursor();
        else
            UnlockCursor();

        if (startPausing)
            PauseGame();
        else
            ResumeGame();
    }

    void Update()
    {
        frameOpenedTabs = openedTabs.Count;
    }

    public void PushTab(Canvas tab)
    {
        openedTabs.Add(tab);
        Debug.Log(openedTabs.Count);
    }

    public bool PopTab(Canvas tab)
    {
        if (openedTabs.Count > 0 && openedTabs[openedTabs.Count - 1] == tab)
        {
            openedTabs.Remove(tab);
            return true;
        }

        return false;
    }

    public void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
    }

    public void OpenTab(Canvas tab, bool pause = true)
    {
        PushTab(tab);
        if (pause)
            PauseGame();
        UnlockCursor();
        tab.gameObject.SetActive(true);
    }

    public bool CloseTab(Canvas tab)
    {
        if (PopTab(tab))
        {
            ResumeGame();
            LockCursor();
            tab.gameObject.SetActive(false);
            return true;
        }
        return false;
    }
}
