using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Defines the item used for leveling up.
/// </summary>
public class LevelItem : KeyItem
{
    public LevelItem() : base()
    {
        itemTypeID = -2;
        value = 1000;

        stackable = true;
        stashable = false;
        stealable = false;

        names.Add(0, "Magical Orb");

        descriptions.Add(0, "Use to increase your stats. Placeholder text to test horizontal window.");
    }
}
