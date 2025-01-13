using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }

    HashSet<PickupItem> currentKeys = new HashSet<PickupItem>();

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void ObtainKey(PickupItem key)
    {
        currentKeys.Add(key);

        InventoryHUDManager.Instance.AddNewItem(key);
    }

    public bool HasKey(PickupItem key)
    {
        return currentKeys.Contains(key);
    }

    public bool HasAnyKey(PickupItem[] keys)
    {
        if (keys.Length == 0)
            return true;

        foreach (PickupItem key in keys)
        {
            if (currentKeys.Contains(key))
                return true;
        }
        return false;
    }
}
