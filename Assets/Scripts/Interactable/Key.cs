using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour, IInteractable
{
    public void Interact()
    {
        InventoryManager.Instance.ObtainKey(this);
        gameObject.SetActive(false);
    }
}
