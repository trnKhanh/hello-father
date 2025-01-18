using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudioManager : MonoBehaviour
{
    [Header("Audio Clips")]
    public AudioClip walkingSFX;
    public AudioClip sprintingSFX;
    public AudioClip dieSFX;

    private bool died = false;

    AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        audioSource.enabled = Time.timeScale != 0;

        if (audioSource.enabled)
        {
            if (!audioSource.isPlaying && !died)
                audioSource.Play();
        }
    }

    public void Die()
    {
        if (died)
            return;

        died = true;
        audioSource.clip = dieSFX;
        audioSource.loop = false;
        audioSource.Play();

    }
    public void Sprinting()
    {
        if (died)
            return;

        audioSource.clip = sprintingSFX;
        audioSource.loop = true;
    }

    public void Walking()
    {
        if (died)
            return;

        audioSource.clip = walkingSFX;
        audioSource.loop = true;
    }

    public void Stop()
    {
        if (died)
            return;

        audioSource.clip = null;
    }
}
