using System;
using System.Collections.Generic;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;
    [SerializeField] private List<InventoryObject> inventory = new List<InventoryObject>();

    private void Awake()
    {
        Instance = this;
    }

    public void AddItemToInventory(DatabaseObject newItem)
    {
        int newItemIndex = FindItemByID(newItem.ID);
        if (newItemIndex >= 0)
        {
            inventory[newItemIndex].quantity++;
        }
        else
        {
            InventoryObject newItemObject = new InventoryObject();
            newItemObject.objectData = newItem;
            newItemObject.quantity = 1;
            inventory.Add(newItemObject);
        }
    }

    public void RemoveItemFromInventory(DatabaseObject itemToRemove)
    {
        int newItemIndex = FindItemByID(itemToRemove.ID);

        if (FindItemByID(itemToRemove.ID) >= 0)
        {
            inventory[newItemIndex].quantity--;

            if(inventory[newItemIndex].quantity <= 0)
            {
                inventory.RemoveAt(newItemIndex);
            }
        }
    }

    private int FindItemByID(int ID)
    {
        return inventory.FindIndex(data => data.objectData.ID == (ID));
    }

    public List<InventoryObject> GetInventory()
    {
        return inventory;
    }
}

[Serializable]
public class InventoryObject
{
    public DatabaseObject objectData;
    public int quantity;
}
