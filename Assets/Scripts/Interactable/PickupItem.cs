using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupItem : MonoBehaviour, IInteractable
{
    [Header("Description")]
    public Sprite itemIcon;
    public string itemName;
    public string description;

    public void Interact()
    {
        PlaySound();
        InventoryManager.Instance.ObtainKey(this);
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
