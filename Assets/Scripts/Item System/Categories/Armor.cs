using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Stats;

/// <summary>
/// Base class for all armor.
/// </summary>
public abstract class Armor : Item
{
    public Armor()
    {
        SetRequirements(0, 0, 0);

        baseStats = new Dictionary<Damage, int>()
        {
            { Damage.Physical, 0 },
            { Damage.Fire, 0 }, { Damage.Frost, 0 }, { Damage.Lightning, 0 },
            { Damage.Magic, 0 }, { Damage.Holy, 0 }, { Damage.Dark, 0 }
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

    public override Dictionary<Damage, int> GetStats(int str, int dex, int @int)
    {
        return baseStats;
    }
}
