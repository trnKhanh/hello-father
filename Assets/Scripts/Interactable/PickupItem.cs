using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupItem : MonoBehaviour, IInteractable
{
    [Header("Description")]
    public int id;
    public Sprite itemIcon;
    public string itemName;
    public string description;

    public static PickupItem GetById(int id)
    {
        PickupItem[] items = FindObjectsOfType<PickupItem>();
        foreach (PickupItem item in items)
        {
            if (item.id == id)
                return item;
        }
        return null;
    }

    public static void RemoveById(int id)
    {
        PickupItem[] items = FindObjectsOfType<PickupItem>();
        foreach (PickupItem item in items)
        {
            if (item.id == id)
                item.gameObject.SetActive(false);
        }
    }

    public void Interact()
    {
        PlaySound();
        InventoryManager.Instance.ObtainKey(id);
        Destroy(gameObject);
    }

    public string GetDescription()
    {
        return description;
    }

    public void PlaySound()
    {
        AudioManager.Instance.PlaySFX(AudioManager.SFXState.KeyPickup);
    }
}
