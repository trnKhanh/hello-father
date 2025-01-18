using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour, IInteractable
{
    [Header("References")]
    public PageController readableObject;

    [Header("Description")]
    public string description;

    public void Interact()
    {
        ReadingManager.Instance.StartRead(readableObject);
    }

    public string GetDescription()
    {
        return description;
    }
}
