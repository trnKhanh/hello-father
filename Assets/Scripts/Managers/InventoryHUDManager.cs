using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryHUDManager : MonoBehaviour
{
    public static InventoryHUDManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    [Header("Prefabs")]
    public GameObject itemUIPrefab;

    [Header("References")]
    public RectTransform itemListPanel;

    public void AddNewItem(PickupItem item)
    {
        GameObject obj = Instantiate(itemUIPrefab, itemListPanel);

        obj.GetComponent<PickupItemUI>().SetUI(item.icon, item.name);
    }
}
