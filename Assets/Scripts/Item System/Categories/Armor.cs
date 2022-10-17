using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base class for all armor.
/// </summary>
public abstract class Armor : Item
{
    protected override string defaultSpritePath => "Graphics/Items/Item Categories/Category Armor";

    public Armor()
    {
        SetRequirements(0, 0, 0);

        baseStats = new Dictionary<Stats.Damage, int>()
        {
            { Stats.Damage.Physical, 0 },
            { Stats.Damage.Fire, 0 }, { Stats.Damage.Frost, 0 }, { Stats.Damage.Lightning, 0 },
            { Stats.Damage.Magic, 0 }, { Stats.Damage.Holy, 0 }, { Stats.Damage.Dark, 0 }
        };
    }

    public static string GetStaticItemClass()
    {
        return "Armor";
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
