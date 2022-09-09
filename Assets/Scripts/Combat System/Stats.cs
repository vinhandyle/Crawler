using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Defines the terminology for combat mechanics.
/// </summary>
public class Stats : MonoBehaviour
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

    /// <summary>
    /// The 9 damage types.
    /// </summary>
    public enum Damage
    {
        Slash,
        Strike,
        Pierce,
        Fire,
        Cold,
        Lightning,
        Magic,
        Holy,
        Dark
    }
}
