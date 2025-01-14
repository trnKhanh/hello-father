using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour, IInteractable
{
    [Header("References")]
    public PageController readableObject;


    public void Interact()
    {
        ReadingManager.Instance.StartRead(readableObject);
    }
}
