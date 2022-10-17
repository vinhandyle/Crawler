using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base class for all ammunition.
/// </summary>
public abstract class Ammo : Item
{
    protected override string defaultSpritePath => "Graphics/Items/Item Categories/Category Ammo";

    protected Dictionary<Stats.Damage, float[]> scalings = new Dictionary<Stats.Damage, float[]>()
    {
        { Stats.Damage.Physical, new float[3] { 0, 0, 0 } },
        { Stats.Damage.Fire, new float[3] { 0, 0, 0 } }, { Stats.Damage.Frost, new float[3] { 0, 0, 0 } }, { Stats.Damage.Lightning, new float[3] { 0, 0, 0 } },
        { Stats.Damage.Magic, new float[3] { 0, 0, 0 } }, { Stats.Damage.Holy, new float[3] { 0, 0, 0 } }, { Stats.Damage.Dark, new float[3] { 0, 0, 0 } }
    };

    public Ammo(int quantity)
    {
        stackable = true;
        this.quantity = quantity;

        baseStats = new Dictionary<Stats.Damage, int>()
        {
            { Stats.Damage.Physical, 0 },
            { Stats.Damage.Fire, 0 }, { Stats.Damage.Frost, 0 }, { Stats.Damage.Lightning, 0 },
            { Stats.Damage.Magic, 0 }, { Stats.Damage.Holy, 0 }, { Stats.Damage.Dark, 0 }
        };
    }

    public static string GetStaticItemClass()
    {
        return "Ammo";
    }

    public override string GetItemClass()
    {
        return GetStaticItemClass();
    }

    public override Dictionary<Stats.Damage, int> GetStats(int str, int dex, int @int)
    {
        return baseStats;
    }
}
