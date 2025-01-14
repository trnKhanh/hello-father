using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSManager : MonoBehaviour
{
    static public FPSManager Instance { get; private set; }

    [Header("FPS settings")]
    public int defaultFPS = 60;
    public int defaultVSyncCount = 0;

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
        QualitySettings.vSyncCount = defaultVSyncCount;
        Application.targetFrameRate = defaultFPS;
    }
}
