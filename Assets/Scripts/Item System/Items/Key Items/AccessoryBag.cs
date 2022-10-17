using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Defines the item used to track the number of accessory slots.
/// </summary>
public class AccessoryBag : KeyItem
{
    public AccessoryBag(int quantity) : base()
    {
        itemTypeID = -2;
        value = 100000;
        sprite = GetSprite("AccessoryBag");

        stackable = true;
        stashable = false;
        stealable = false;
        this.quantity = quantity;

        names.Add(0, "Accessory Pouch");

        descriptions.Add(0, "Used to equip one additional accessory.");
    }
}
