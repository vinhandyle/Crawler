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
    [SerializeField] private List<string> itemNames;
    private List<Item> items = new List<Item>();
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

    public int accessorySlots
    {
        get
        {
            Item item = items.Find(i => i.itemTypeID == -2);
            return 1 + ((item == null) ? 0 : item.quantity);
        }
    }

    public int availableLvls
    {
        get
        {
            Item item = items.Find(i => i.itemTypeID == -3);
            return (item == null) ? 0 : item.quantity;
        }
    }

    #endregion

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
            itemNames.Add(item.ToString());

            if (item.GetItemClass() == Weapon.GetStaticItemClass())
            {
                Technique tech = ((Weapon)item).tech;
                if (!itemNames.Contains(tech?.ToString()))
                    AddItem(((Weapon)item).tech);
            }
        }
    }

    /// <summary>
    /// Adds the specified item to the inventory based on the given probability.
    /// </summary>
    public void AddItemChance(Item item, float chance)
    {
        System.Random rand = new System.Random();
        if (rand.NextDouble() >= chance)
            AddItem(item);
    }

    /// <summary>
    /// Removes the specified item from the inventory.
    /// </summary>
    public void RemoveItem(Item item)
    {
        if (item != null && items.Contains(item))
        {
            items.Remove(item);
            itemNames.Remove(item.ToString());
        }
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
            if (items.Find(i => i.GetType() == typeof(GoldCoin)) == null && price > 0)
            {
                items.Add(new GoldCoin(0));
            }

            dest.coins -= price;
            coins += price;

            // Adjust item in both inventories
            if (item.stackable)
            {
                if (dest.items.Any(i => i.itemTypeID == item.itemTypeID))
                {
                    dest.items[dest.items.FindIndex(i => i.itemTypeID == item.itemTypeID)].quantity += amt;
                }
                else
                {
                    Item newItem = item.Clone();
                    newItem.quantity = amt;                  
                    dest.AddItem(newItem);
                }

                item.quantity -= amt;
                if (item.quantity == 0)
                {
                    RemoveItem(item);
                }
            }
            else
            {
                dest.AddItem(item);
                RemoveItem(item);
            }
        }
        return successful;
    }
}
