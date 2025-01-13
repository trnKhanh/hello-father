using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupItem : MonoBehaviour, IInteractable
{
    [Header("Icon and Name")]
    public Sprite icon;
    public string name;

    public void Interact()
    {
        InventoryManager.Instance.ObtainKey(this);
        Destroy(gameObject);
    }
}
