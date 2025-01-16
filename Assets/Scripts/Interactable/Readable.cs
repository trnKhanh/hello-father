using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Readable : MonoBehaviour, IInteractable
{
    [Header("References")]
    public PageController pageController;

    public void Interact()
    {
        ReadingManager.Instance.StartRead(pageController);
    }
}
