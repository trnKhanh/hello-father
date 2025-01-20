using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{
    [Header("Audio")]
    public AudioClip openSFX;
    public AudioClip closeSFX;
    public AudioClip lockedSFX;

    [Header("Key")]
    public PickupItem[] requiredKeys;

    [Header("Description")]
    public string description = "Open door";

    bool isClosed = true;
    public bool canOpened;

    Animator animator;
    AudioSource audioSource;

    string k_interact = "Interact";


    void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    public void Interact()
    {
        if (InventoryManager.Instance.HasAnyKey(requiredKeys))
        {
            if (isClosed)
                PlayOpenSound();
            else
                PlayCloseSound();
                    
            isClosed = !isClosed;
            animator.SetTrigger(k_interact);
        } else
        {
            PlayLockedSound();
        }
    }

    public bool NeedKey()
    {
        return requiredKeys.Length > 0;
    }

    public void Open()
    {
        if (!canOpened)
            return;

        if (isClosed)
        {
            Interact();
        }
    }

    public void Close()
    {
        if (!isClosed)
        {
            Interact();
        }
    }

    public string GetDescription()
    {
        return description;
    }

    public void PlayOpenSound()
    {
        audioSource.clip = openSFX;
        audioSource.Play();
    }

    public void PlayCloseSound()
    {
        audioSource.clip = closeSFX;
        audioSource.Play();
    }

    public void PlayLockedSound()
    {
        audioSource.clip = lockedSFX;
        audioSource.Play();
    }
}
