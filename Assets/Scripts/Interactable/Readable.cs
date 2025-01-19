using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Readable : MonoBehaviour, IInteractable
{
    [Header("References")]
    public PageController pageController;

    [Header("Description")]
    public string description;

    public void Interact()
    {
        ReadingManager.Instance.StartRead(pageController);
        PlaySound();
    }

    public string GetDescription()
    {
        return description;
    }

    public void PlaySound()
    {
        AudioManager.Instance.PlaySFX(AudioManager.SFXState.TurnPage);
    }
}
