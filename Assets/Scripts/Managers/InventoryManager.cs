using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class InventoryManager : MonoBehaviour, IGameData
{
    public static InventoryManager Instance { get; private set; }

    [Serializable]
    public class InventoryData
    {
        public List<int> currentKeys = new List<int>();
    }

    private InventoryData inventoryData = new InventoryData();

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void ObtainKey(int id)
    {
        if (inventoryData.currentKeys.Contains(id))
            return;

        inventoryData.currentKeys.Add(id);

        InventoryHUDManager.Instance.AddNewItem(PickupItem.GetById(id));
    }

    public bool HasKey(int id)
    {
        return inventoryData.currentKeys.Contains(id);
    }

    public bool HasAnyKey(PickupItem[] keys)
    {
        if (keys.Length == 0)
            return true;

        foreach (PickupItem key in keys)
        {
            if (HasKey(key.id))
                return true;
        }
        return false;
    }

    public void Save(string root)
    {
        string savePath = Path.Join(root, "inventory.json");

        Debug.Log(String.Format("Save Inventory to {0}", savePath));
        File.WriteAllText(savePath, JsonUtility.ToJson(inventoryData));
    }

    public void Load(string root)
    {
        try
        {
            string savePath = Path.Join(root, "inventory.json");
            InventoryData data = JsonUtility.FromJson<InventoryData>(File.ReadAllText(savePath));

            foreach (int id in data.currentKeys)
            {
                ObtainKey(id);
                PickupItem.RemoveById(id);
            }
        }
        catch (Exception e)
        {
            Debug.LogWarning(e);
        }
    }
}
