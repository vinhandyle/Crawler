using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base class for all techniques.
/// </summary>
public abstract class Technique : Item
{
    protected override string defaultSpritePath => "Graphics/Items/Item Categories/Category Technique";

    public Stats.AttackType type { get => _type; set => _type = value; }
    [SerializeField] protected Stats.AttackType _type;

    public Technique()
    {
        spritePath += "Techniques/";

        stashable = false;
        sellable = false;
        stealable = false;

        SetRequirements(0, 0, 0);
        SetUseCosts(0, 0, 0);
    }

    public static string GetStaticItemClass()
    {
        return "Technique";
    }

    public override string GetItemClass()
    {
        return GetStaticItemClass();
    }

    /// <summary>
    /// Check if this technique can be linked to the specified weapon.
    /// </summary>
    public virtual bool CheckValidLink(Weapon weapon)
    {
        return _type == weapon.type;
    }
}
