using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    public enum BackgroundState
    {
        Menu,
        Gameplay,
        Gameover,
    }
    public enum SFXState
    {
        TurnPage,
        Click,
        Hover,
        KeyPickup,
        OpenBook,
    }

    [Serializable]
    public struct BackgroundAudioClip
    {
        public BackgroundState state;
        public AudioClip audioClip;
    }

    [Serializable]
    public struct SFXAudioClip
    {
        public SFXState state;
        public AudioClip audioClip;
    }

    [Header("Audio Clips")]
    public BackgroundAudioClip[] backgroundAudioClips;
    public SFXAudioClip[] sfxAudioClips;

    [Header("References")]
    public AudioSource backgroundAudioSource;
    public AudioSource sfxAudioSource;

    [Header("Sound Settings")]
    public BackgroundState defaultBackgroundMusic = BackgroundState.Menu;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        PlayBackgroundMusic(defaultBackgroundMusic);
    }

    public void PlayBackgroundMusic(BackgroundState state)
    {
        AudioClip audioClip = GetBackgroundMusicAudioClip(state);
        backgroundAudioSource.clip = audioClip;
        backgroundAudioSource.Play();
    }

    private AudioClip GetBackgroundMusicAudioClip(BackgroundState state)
    {
        foreach (BackgroundAudioClip clip in backgroundAudioClips)
        {
            if (state == clip.state)
                return clip.audioClip;
        }
        return null;
    }

    public void PlaySFX(SFXState state)
    {
        AudioClip audioClip = GetSFXAudioClip(state);
        sfxAudioSource.PlayOneShot(audioClip);
    }

    private AudioClip GetSFXAudioClip(SFXState state)
    {
        foreach (SFXAudioClip clip in sfxAudioClips)
        {
            if (state == clip.state)
                return clip.audioClip;
        }
        return null;
    }
}
