using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PickupItemUI : MonoBehaviour
{
    [Header("References")]
    public Image itemIconUI;
    public TMP_Text itemNameUI;

    public void SetUI(Sprite icon, string name)
    {
        if (icon != null && itemIconUI != null)
            itemIconUI.sprite = icon;

        if (name != null && itemNameUI != null)
            itemNameUI.text = name;
    }
}
