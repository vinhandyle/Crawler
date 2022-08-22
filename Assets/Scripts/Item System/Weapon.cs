using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Stats;

/// <summary>
/// Base class for all weapons.
/// </summary>
public abstract class Weapon : Item
{
    protected Dictionary<Damage, float[]> scalings = new Dictionary<Damage, float[]>()
    {
        { Damage.Slash, new float[3] { 0, 0, 0 } }, { Damage.Strike, new float[3] { 0, 0, 0 } }, { Damage.Pierce, new float[3] { 0, 0, 0 } },
        { Damage.Fire, new float[3] { 0, 0, 0 } }, { Damage.Cold, new float[3] { 0, 0, 0 } }, { Damage.Lightning, new float[3] { 0, 0, 0 } },
        { Damage.Magic, new float[3] { 0, 0, 0 } }, { Damage.Holy, new float[3] { 0, 0, 0 } }, { Damage.Dark, new float[3] { 0, 0, 0 } }
    };

    public override void SetBaseInfo()
    {
        base.SetBaseInfo();

        baseStats = new Dictionary<Damage, int>()
        {
            { Damage.Slash, 0 }, { Damage.Strike, 0 }, { Damage.Pierce, 0 },
            { Damage.Fire, 0 }, { Damage.Cold, 0 }, { Damage.Lightning, 0 },
            { Damage.Magic, 0 }, { Damage.Holy, 0 }, { Damage.Dark, 0 }
        };
    }

    /// <summary>
    /// Returns the stat distribution of this weapon from scaling only.
    /// </summary>
    public Dictionary<Damage, int> GetScaledStats(int str, int dex, int @int)
    {
        Dictionary<Damage, int> stats = new Dictionary<Damage, int>();
        foreach (Damage name in baseStats.Keys)
        {
            stats.Add(name, GetScaledStat(reqs, new int[3] { str, dex, @int }, scalings[name]));
        }
        return stats;
    }

    /// <summary>
    /// Returns the stat distribution of this weapon after scaling.
    /// </summary>
    public override Dictionary<Damage, int> GetStats(int str, int dex, int @int)
    {
        Dictionary<Damage, int> stats = GetScaledStats(str, dex, @int);
        foreach (Damage name in baseStats.Keys)
        {
            stats[name] += baseStats[name];
        }
        return stats;
    }
}
