using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base class for upgrade materials.
/// </summary>
public abstract class Material : Item
{
    protected override string defaultSpritePath => "Graphics/Items/Item Categories/Category Material";

    public Material(int quantity) : base()
    {
        stackable = true;
        this.quantity = quantity;
    }

    public static string GetStaticItemClass()
    {
        return "Material";
    }

    public override string GetItemClass()
    {
        return GetStaticItemClass();
    }
}
