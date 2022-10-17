using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base class for all weapons.
/// </summary>
public abstract class Weapon : Item
{
    protected override string defaultSpritePath => "Graphics/Items/Item Categories/Category Weapon";

    protected Dictionary<Stats.Damage, float[]> scalings = new Dictionary<Stats.Damage, float[]>()
    {
        { Stats.Damage.Physical, new float[3] { 0, 0, 0 } },
        { Stats.Damage.Fire, new float[3] { 0, 0, 0 } }, { Stats.Damage.Frost, new float[3] { 0, 0, 0 } }, { Stats.Damage.Lightning, new float[3] { 0, 0, 0 } },
        { Stats.Damage.Magic, new float[3] { 0, 0, 0 } }, { Stats.Damage.Holy, new float[3] { 0, 0, 0 } }, { Stats.Damage.Dark, new float[3] { 0, 0, 0 } }
    };

    [SerializeField] protected int baseSpellDmg;
    [SerializeField] protected float spellScaling;

    public Stats.AttackType type { get => _type; set => _type = value; }
    [SerializeField] protected Stats.AttackType _type;

    public Technique tech = null;

    public Weapon()
    {
        spritePath += "Weapons/";
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
        return "Weapon";
    }

    public override string GetItemClass()
    {
        return GetStaticItemClass();
    }

    /// <summary>
    /// Returns the stat distribution of this weapon from scaling only.
    /// </summary>
    public Dictionary<Stats.Damage, int> GetScaledStats(int str, int dex, int @int)
    {
        Dictionary<Stats.Damage, int> stats = new Dictionary<Stats.Damage, int>();
        foreach (Stats.Damage name in baseStats.Keys)
        {
            stats.Add(name, GetScaledStat(reqs, new int[3] { str, dex, @int }, scalings[name]));
        }
        return stats;
    }

    /// <summary>
    /// Returns the stat distribution of this weapon after scaling.
    /// </summary>
    public override Dictionary<Stats.Damage, int> GetStats(int str, int dex, int @int)
    {
        Dictionary<Stats.Damage, int> stats = GetScaledStats(str, dex, @int);
        foreach (Stats.Damage name in baseStats.Keys)
        {
            stats[name] += baseStats[name];
        }
        return stats;
    }

    /// <summary>
    /// Returns the spell damage of this weapon scaled on the given intelligence level.
    /// </summary>
    public virtual float GetSpellDamage(int @int)
    {
        return baseSpellDmg + spellScaling * @int;
    }
}
