using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    private static Inventory instance;

    private HashSet<string> items = new HashSet<string>();

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public static bool HasItem(string itemID)
    {
        return instance.items.Contains(itemID);
    }

    public static void AddItem(string itemID)
    {
        if (!instance.items.Contains(itemID))
        {
            instance.items.Add(itemID);
            Debug.Log($"Item {itemID} added to inventory.");
        }
    }
}