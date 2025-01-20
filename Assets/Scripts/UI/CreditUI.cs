using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditUI : MonoBehaviour
{
    void Start()
    {
        GameDataManager.Instance.DeleteSaveFile();    
    }

    void Update()
    {
        if (InputManager.Instance.escape)
        {
            SceneStateManager.Instance.LoadMenu();
        }
    }

    public void GoToMenu()
    {
        SceneStateManager.Instance.LoadMenu();
    }
}
