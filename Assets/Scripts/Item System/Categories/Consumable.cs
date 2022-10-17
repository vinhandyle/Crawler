using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base class for consumable items, including those with infinite uses.
/// </summary>
public abstract class Consumable : Item
{
    protected override string defaultSpritePath => "Graphics/Items/Item Categories/Category Consumable";

    public Consumable()
    { 
    
    }

    public static string GetStaticItemClass()
    {
        return "Consumable";
    }

    public override string GetItemClass()
    {
        return GetStaticItemClass();
    }

    /// <summary>
    /// Use the specified amount of this item.
    /// </summary>
    public abstract void Use(int amt);
}
