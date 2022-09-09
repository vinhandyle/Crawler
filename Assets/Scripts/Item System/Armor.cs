using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Stats;

/// <summary>
/// Base class for all armor.
/// </summary>
public abstract class Armor : Item
{
    public override Dictionary<Damage, int> GetStats(int str, int dex, int @int)
    {
        return baseStats;
    }
}
