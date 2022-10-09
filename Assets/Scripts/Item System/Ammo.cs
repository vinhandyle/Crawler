using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Stats;

/// <summary>
/// Base class for all ammunition.
/// </summary>
public abstract class Ammo : Item
{
    protected Dictionary<Damage, float[]> scalings = new Dictionary<Damage, float[]>()
    {
        { Damage.Physical, new float[3] { 0, 0, 0 } },
        { Damage.Fire, new float[3] { 0, 0, 0 } }, { Damage.Frost, new float[3] { 0, 0, 0 } }, { Damage.Lightning, new float[3] { 0, 0, 0 } },
        { Damage.Magic, new float[3] { 0, 0, 0 } }, { Damage.Holy, new float[3] { 0, 0, 0 } }, { Damage.Dark, new float[3] { 0, 0, 0 } }
    };

    public override void SetBaseInfo()
    {
        base.SetBaseInfo();
        stackable = true;

        baseStats = new Dictionary<Damage, int>()
        {
            { Damage.Physical, 0 },
            { Damage.Fire, 0 }, { Damage.Frost, 0 }, { Damage.Lightning, 0 },
            { Damage.Magic, 0 }, { Damage.Holy, 0 }, { Damage.Dark, 0 }
        };
    }

    public override Dictionary<Stats.Damage, int> GetStats(int str, int dex, int @int)
    {
        return baseStats;
    }
}
