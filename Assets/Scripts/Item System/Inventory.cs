﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Defines the inventory space of any character.
/// </summary>
public class Inventory : MonoBehaviour
{
    [SerializeField] private List<Item> items;
    public int coins;
    public bool standalone; // Whether the inventory is not associated to a profile

    private void Awake()
    {
        List<Item> _items = new List<Item>();
        foreach (Item item in items)
        {
            // We are assuming all stackable items were manually added into the scene
            if (item.stackable)
            {
                item.SetBaseInfo();
                _items.Add(item);
            }
            else
            {
                Item _item = item.CloneFromPrefab();
                _items.Add(_item);
            }
        }
        items = _items;
    }

    /// <summary>
    /// Returns all items in this inventory.
    /// </summary>
    public List<Item> GetAllItems()
    {
        return items;
    }

    /// <summary>
    /// Sorts the inventory by item type.
    /// </summary>
    public void Sort()
    {
        items.OrderBy(item => item.itemTypeID);
    }

    /// <summary>
    /// Move an item from this inventory to the destination inventory. 
    /// Returns true if successful.
    /// </summary>
    public bool TransferItem(Inventory dest, Item item, string type, int amt)  
    {
        bool successful = true;
        int price = 0;
        int index = items.IndexOf(item);

        switch (type)
        {
            case "Buy":
                price = item.value * amt;
                successful = coins >= price;
                break;

            case "Sell":
                price = -(int)(item.value * 0.8f) * amt;
                successful = dest.coins >= Mathf.Abs(price);
                break;

            case "Steal":
                successful = true;
                break;
        }

        if (successful)
        {
            coins -= price;
            dest.coins += price;

            // Adjust item in both inventories
            if (item.stackable)
            {
                if (dest.items.Any(i => i.itemTypeID == item.itemTypeID))
                {
                    Debug.Log(dest.items.FindIndex(i => i.itemTypeID == item.itemTypeID));
                    dest.items[dest.items.FindIndex(i => i.itemTypeID == item.itemTypeID)].quantity += amt;
                }
                else
                {
                    Item newItem = item.CloneFromPrefab();
                    newItem.stackable = true;
                    newItem.quantity = amt;
                    dest.items.Add(newItem);
                }

                item.quantity -= amt;
                if (item.quantity == 0)
                {
                    items.Remove(item);
                    Destroy(item.gameObject);
                }
            }
            else
            {
                dest.items.Add(item);
                items.Remove(item);
            }
        }
        return successful;
    }
}