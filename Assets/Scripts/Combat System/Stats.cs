using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Defines the terminology for combat mechanics.
/// </summary>
public class Stats
{
    /// <summary>
    /// The 6 levelable stats.
    /// </summary>
    public enum Stat
    {
        Int,
        Arc,
        Dex,
        Agi,
        Str,
        Vit
    }

    /// <summary>
    /// The 3 resources used in combat. 
    /// </summary>
    public enum Gauge
    {
        HP,
        MP,
        SP
    }

    public Dictionary<Gauge, Color> gaugeColor = new Dictionary<Gauge, Color>()
    {
        { Gauge.HP, new Color(1, 0, 0) },
        { Gauge.MP, new Color(0, 0, 1) },
        { Gauge.SP, new Color(0, 1, 0.25f) }
    };

    /// <summary>
    /// The 9 damage types.
    /// </summary>
    public enum Damage
    {
        Physical,
        Fire,
        Frost,
        Lightning,
        Magic,
        Holy,
        Dark
    }

    /// <summary>
    /// The 3 types of non-magic attacks.
    /// </summary>
    public enum AttackType
    {
        Strike,
        Slash,
        Pierce    
    };

    public Dictionary<Damage, Color> dmgColor = new Dictionary<Damage, Color>()
    {
        { Damage.Physical, new Color(0.733f, 0.733f, 0.784f) },
        { Damage.Fire, new Color(1, 0.5f, 0) },
        { Damage.Frost, new Color(0, 1, 1) },
        { Damage.Lightning, new Color(1, 1, 0) },
        { Damage.Magic, new Color(0.5f, 0, 1) },
        { Damage.Holy, new Color(1, 1, 1) },
        { Damage.Dark, new Color(0, 0, 0) }
    };
}
