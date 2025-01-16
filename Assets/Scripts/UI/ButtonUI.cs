using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class ButtonUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerUpHandler, IPointerDownHandler
{
    [Header("Materials")]
    public Material hoverMaterial;
    public Material clickMaterial;
    Material startMaterial;

    bool hovering = false;
    bool clicking = false;

    TMP_Text textMeshPro;
    Button button;

    void Awake()
    {
        textMeshPro = GetComponentInChildren<TMP_Text>();
        button = GetComponent<Button>();
    }

    void Start()
    {
        startMaterial = textMeshPro.fontSharedMaterial;
    }

    void OnEnable()
    {
        hovering = clicking = false;

        button.onClick.AddListener(button_onClick);
    }

    void OnDisable()
    {
        button.onClick.RemoveListener(button_onClick);
    }

    void Update()
    {
        if (clicking)
        {
            SetMaterial(clickMaterial);
        } else
        {
            if (hovering)
            {
                SetMaterial(hoverMaterial);
            } else
            {
                SetMaterial(startMaterial);
            }
        }
    }

    void SetMaterial(Material material)
    {
        textMeshPro.fontSharedMaterial = material;
    }

    public void button_onClick()
    {
        AudioManager.Instance.PlaySFX(AudioManager.SFXState.Click);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        hovering = true;
        AudioManager.Instance.PlaySFX(AudioManager.SFXState.Hover);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        hovering = false;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        clicking = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        clicking = false;
    }


}
