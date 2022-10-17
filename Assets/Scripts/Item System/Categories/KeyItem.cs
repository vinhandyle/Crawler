using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base class for items that the player cannot throw away.
/// </summary>
public abstract class KeyItem : Item
{
    protected override string defaultSpritePath => "Graphics/Items/Item Categories/Category Key Item";

    public KeyItem()
    {
        sellable = false;
    }

    public static string GetStaticItemClass()
    {
        return "KeyItem";
    }

    public override string GetItemClass()
    {
        return GetStaticItemClass();
    }
}
