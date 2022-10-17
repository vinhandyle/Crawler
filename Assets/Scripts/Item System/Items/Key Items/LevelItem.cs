using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Defines the item used for leveling up.
/// </summary>
public class LevelItem : KeyItem
{
    public LevelItem(int quantity) : base()
    {
        itemTypeID = -3;
        value = 1000;
        sprite = GetSprite("LevelItem");

        stackable = true;
        stashable = false;
        stealable = false;
        this.quantity = quantity;

        names.Add(0, "Magical Orb");

        descriptions.Add(0, "Use to increase your stats.");
    }
}
