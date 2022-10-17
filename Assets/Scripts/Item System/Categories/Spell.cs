using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base class for all spells.
/// </summary>
public abstract class Spell : Item
{
    protected override string defaultSpritePath => "Graphics/Items/Item Categories/Category Spell";

    public Spell()
    {
        stashable = false;
        sellable = false;
        stealable = false;

        SetRequirements(0, 0, 0);
        SetUseCosts(0, 0, 0);
    }

    public static string GetStaticItemClass()
    {
        return "Spell";
    }

    public override string GetItemClass()
    {
        return GetStaticItemClass();
    }
}
