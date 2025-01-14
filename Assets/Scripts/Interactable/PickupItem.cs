using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupItem : MonoBehaviour, IInteractable
{
    [Header("Icon and Name")]
    public Sprite itemIcon;
    public string itemName;

    public void Interact()
    {
        InventoryManager.Instance.ObtainKey(this);
        Destroy(gameObject);
    }
}
