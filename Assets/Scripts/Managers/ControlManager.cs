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
    Canvas tabsToClose;
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

    private void LateUpdate()
    {
        if (tabsToClose)
        {
            openedTabs.Remove(tabsToClose);
            tabsToClose = null;
        }
    }

    public void PushTab(Canvas tab)
    {
        openedTabs.Add(tab);
    }

    public bool PopTab(Canvas tab)
    {
        if (tabsToClose == null && openedTabs.Count > 0 && openedTabs[openedTabs.Count - 1] == tab)
        {
            tabsToClose = tab;
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
