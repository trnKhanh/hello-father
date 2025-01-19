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
            animator.SetTrigger(k_interact);
        } else
        {
            PlayLockedSound();
        }
    }

    public void Open()
    {
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
