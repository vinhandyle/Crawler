using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Consumable : Item
{
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
