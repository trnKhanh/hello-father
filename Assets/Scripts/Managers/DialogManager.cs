using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour
{
    public enum Character
    {
        MainCharacter,
        Father
    };

    [Serializable]
    public class CharacterData
    {
        public Character character;
        public string name = null;
        public Sprite avatar = null;
        public AudioClip audioClip = null;
    }


    [Serializable]
    public struct Dialog
    {
        public Character character;
        public string text;
        public AudioClip sound;
    }

    static public DialogManager Instance { get; private set; }

    static public Action<int> m_onFinishedDialog = null;

    [Header("Characters")]
    public CharacterData[] npcData;

    [Header("Keybinds")]
    public KeyCode[] skipKeys;

    [Header("Dialog Settings")]
    public float textDelay = 0.1f;
    Dialog[] m_dialogs = null;
    Coroutine m_animateTextCoroutine = null;
    int m_curDialogId;
    float m_closeTimeout = -1;
    bool m_inDialogEffect = false;

    [Header("References")]
    public Image avatarImageUI;
    public TMP_Text dialogTextUI;
    public TMP_Text nameTextUI;
    public RectTransform dialogPanelUI;
    AudioSource m_audioSource;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
            return;
        }
        Instance = this;
    }

    void Start()
    {
        m_audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        // Check if the player want to skip the dialog
        bool skip = false;
        foreach (KeyCode key in skipKeys)
        {
            skip |= Input.GetKeyDown(key);
        }
        if (skip)
            Skip();
    }

    // Player the dialogs in the listed orders. If timeout > 0, the dialogs will be closed automatically after the timeout
    public void PlayDialogs(List<Dialog> dialogs, Action<int> callback = null, float timeout = -1)
    {
        SetDialogs(dialogs.ToArray(), callback, timeout);
        PlayNextDialog();
    }

    void SetDialogs(Dialog[] dialogs, Action<int> callback = null, float timeout = -1)
    {
        m_curDialogId = 0;
        m_dialogs = dialogs;
        m_closeTimeout = timeout;
        m_onFinishedDialog = callback;
    }

    void ResetDialogs()
    {
        m_curDialogId = 0;
        m_dialogs = null;
        m_closeTimeout = -1;
        m_onFinishedDialog = null;
    }

    void PlayNextDialog()
    {
        if (m_dialogs != null && m_curDialogId < m_dialogs.Length)
        {
            // Player the next dialog in list if there are more.
            m_animateTextCoroutine = StartCoroutine(AnimateDialog(m_dialogs[m_curDialogId]));
        }
        else
        {
            // Reset the state of the diaglos and hide the UI.
            ResetDialogs();
            HideDialog();
        }
    }

    IEnumerator AnimateDialog(Dialog dialog)
    {
        InitDialogEffect(dialog);

        // Typing effect
        dialogTextUI.maxVisibleCharacters = 0;
        int total = dialogTextUI.text.Length;

        for (int i = 1; i <= total; ++i)
        {
            dialogTextUI.maxVisibleCharacters = i;
            yield return new WaitForSeconds(textDelay);
        }

        FinishDialogEffect();
    }

    
    // Player the given dialog
    void InitDialogEffect(Dialog dialog)
    {
        m_inDialogEffect = true;
        ShowDialog();

        CharacterData data = GetNPCData(dialog.character);

        // Update the UI
        UpdateDialogUI(dialog);
        UpdateCharacterUI(data);

        // If the dialog has sound, then play it. Otherwise, play default character voice.
        if (dialog.sound != null)
            PlayDialogSound(dialog);
        else
            PlayCharacterVoice(data);
    }

    void UpdateDialogUI(Dialog dialog)
    {
        if (dialogTextUI != null)
            dialogTextUI.text = dialog.text;
    }

    void PlayDialogSound(Dialog dialog)
    {
        m_audioSource.clip = dialog.sound;
        m_audioSource.loop = false;
        m_audioSource.Play();
    }

    void UpdateCharacterUI(CharacterData data)
    {
        if (avatarImageUI != null)
        {
            if (data.avatar != null)
            {
                avatarImageUI.gameObject.SetActive(true);
                avatarImageUI.sprite = data.avatar;
            }
            else
            {
                avatarImageUI.gameObject.SetActive(false);
            }
        }

        if (nameTextUI != null)
        {
            nameTextUI.text = data.name;
        }
    }

    void PlayCharacterVoice(CharacterData data)
    {
        m_audioSource.clip = data.audioClip;
        m_audioSource.loop = true;
        m_audioSource.Play();
    }

    // Finish the current dialog. Stop all the sound and disable 
    void FinishDialogEffect()
    {
        m_inDialogEffect = false;

        m_audioSource.Stop();
        m_animateTextCoroutine = null;

        
        if (m_closeTimeout > 0)
            StartCoroutine(DialogTimeout(m_curDialogId));
    }

    IEnumerator DialogTimeout(int dialogId)
    {
        yield return new WaitForSeconds(m_closeTimeout);
        // If the current dialog is still the same, then play the next dialog automatically
        if (dialogId == m_curDialogId)
            FinishDialog();
    }

    void FinishDialog()
    {
        if (m_onFinishedDialog != null)
        {
            m_onFinishedDialog(m_curDialogId);
        }

        m_curDialogId += 1;

        PlayNextDialog();
    }

    void Skip()
    {
        if (m_inDialogEffect)
        {
            // If player wants to skip during the dialog, stop the animation and show all text.
            SkipAnimation();
            FinishDialogEffect();
        }
        else
        {
            FinishDialog();
        }
    }

    void SkipAnimation()
    {
        if (m_animateTextCoroutine != null)
            StopCoroutine(m_animateTextCoroutine);

        dialogTextUI.maxVisibleCharacters = dialogTextUI.text.Length;
        m_animateTextCoroutine = null;
    }

    void ShowDialog()
    {
        dialogPanelUI.gameObject.SetActive(true);
    }

    void HideDialog()
    {
        dialogPanelUI.gameObject.SetActive(false);
    }

    private CharacterData GetNPCData(Character character)
    {
        foreach (CharacterData data in npcData)
        {
            if (character == data.character)
            {
                return data;
            }
        }
        return null;
    }
}
