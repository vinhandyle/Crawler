using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Defines the inventory space of any character.
/// </summary>
public class Inventory : MonoBehaviour
{
    [SerializeField] private List<Item> items;
    public bool standalone; // Whether the inventory is not associated to a profile

    #region Items tied to resource values

    public int coins
    {
        get
        {
            Item item = items.Find(i => i.itemTypeID == -1);
            return (item == null) ? 0 : item.quantity;
        }
        set
        {
            Item item = items.Find(i => i.itemTypeID == -1);
            if (item != null) item.quantity = value;
        }
    }

    public int availableLvls
    {
        get
        {
            Item item = items.Find(i => i.itemTypeID == -2);
            return (item == null) ? 0 : item.quantity;
        }
        set
        {
            Item item = items.Find(i => i.itemTypeID == -2);
            if (item != null) item.quantity = value;
        }
    }

    #endregion

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

                // Add default linked techniques
                Technique tech = _item.GetComponent<Weapon>()?.tech;
                if (tech)
                {
                    tech.SetBaseInfo();
                    _items.Add(tech);
                }
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
    /// Adds the specified item to the inventory.
    /// </summary>
    public void AddItem(Item item)
    {
        if (item != null)
        {
            items.Add(item);

            Technique tech = item.GetComponent<Weapon>()?.tech;
            if (tech) items.Add(tech);
        }
    }

    /// <summary>
    /// Removes the specified item from the inventory.
    /// </summary>
    public void RemoveItem(Item item)
    {
        if (item != null && items.Contains(item)) items.Remove(item);
    }

    /// <summary>
    /// Move an item from this inventory to the destination inventory. 
    /// Returns true if successful.
    /// </summary>
    public bool TransferItem(Inventory dest, Item item, string type, int amt)  
    {
        var tm = TransferManager.Instance;
        bool successful = true;
        int price = 0;
        int index = items.IndexOf(item);

        switch (type)
        {
            case "Buy":
                price = (int)tm.GetTransferInfo(item, type, amt);
                successful = dest.coins >= price;
                break;

            case "Sell":
                price = (int)tm.GetTransferInfo(item, type, amt);
                successful = dest.coins >= Mathf.Abs(price);
                break;

            case "Steal":
                System.Random rand = new System.Random();
                successful = rand.Next(1, 10000) <= tm.GetTransferInfo(item, type, amt) * 100;
                break;
        }

        if (successful)
        {
            // Add gold item if it isn't in the inventory
            // Required to access and update coin count
            if (!items.Find(i => i.itemTypeID == -1) && price > 0)
            {
                Item goldCoins = dest.items.Find(i => i.itemTypeID == -1).CloneFromPrefab(transform);
                goldCoins.quantity = 0;
                items.Add(goldCoins);
            }

            dest.coins -= price;
            coins += price;

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
                    Item newItem = item.CloneFromPrefab(dest.transform);
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
