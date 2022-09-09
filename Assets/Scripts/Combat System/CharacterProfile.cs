using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Stats;

/// <summary>
/// Defines the numbers of a character.
/// </summary>
public class CharacterProfile : MonoBehaviour
{
    private Dictionary<Stat, int> baseStatPoints = new Dictionary<Stat, int>()
    {
        { Stat.Vit, 0 }, { Stat.Agi, 0 }, { Stat.Arc, 0 },
        { Stat.Str, 0 }, { Stat.Dex, 0 }, { Stat.Int, 0 }
    };

    public Dictionary<Stat, int> statPoints { get; } = new Dictionary<Stat, int>()
    {
        { Stat.Vit, 100 }, { Stat.Agi, 100 }, { Stat.Arc, 50 },
        { Stat.Str, 10 }, { Stat.Dex, 10 }, { Stat.Int, 30 }
    };

    private int statCap = 150;

    #region Skills

    private float _maxBurden = 0;

    public int pickpocket
    {
        get { return (int)(statPoints[Stat.Dex] * pickpocketBonus);  }
    }   
    public float pickpocketBonus;

    #endregion

    #region Base

    /// <summary>
    /// Set all stat levels back to 0;
    /// </summary>
    public void ResetStat()
    {
        foreach (Stat stat in baseStatPoints.Keys)
            baseStatPoints[stat] = 0;
    }

    /// <summary>
    /// Increases the specified stat by 1 level.
    /// </summary>
    public bool LevelUpStat(Stat stat)
    {
        if (baseStatPoints[stat] == 100) return false;
        baseStatPoints[stat]++;
        return true;
    }

    #endregion

    #region Boosted

    /// <summary>
    /// Sets the stat cap to the specified value.
    /// </summary>
    public void SetStatCap(int statCap)
    {
        this.statCap = statCap;
    }

    /// <summary>
    /// 
    /// </summary>
    public void UpdateStat(Stat stat, float mult)
    {
        statPoints[stat] += (int)(statPoints[stat] * mult);
        if (statPoints[stat] > statCap) statPoints[stat] = statCap;
    }

    #endregion
}
