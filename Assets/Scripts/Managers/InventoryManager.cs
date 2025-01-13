using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }

    HashSet<Key> currentKeys = new HashSet<Key>();

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void ObtainKey(Key key)
    {
        currentKeys.Add(key);
    }

    public bool HasKey(Key key)
    {
        return currentKeys.Contains(key);
    }

    public bool HasAnyKey(Key[] keys)
    {
        if (keys.Length == 0)
            return true;

        foreach (Key key in keys)
        {
            if (currentKeys.Contains(key))
                return true;
        }
        return false;
    }
}
