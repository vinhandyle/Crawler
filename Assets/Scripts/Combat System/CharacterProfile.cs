using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Stats;

/// <summary>
/// Defines the base information of a character.
/// </summary>
public class CharacterProfile : MonoBehaviour
{
    #region Name and Type

    public string charName = "Soxar";

    private enum Type
    {
        Normal,
        Boss
    };

    [SerializeField] private Type charType;

    public Color nameColor
    {
        get
        {
            switch (charType)
            {                
                case Type.Boss:
                    return new Color(1, 0, 0);
                               
                default:
                    return new Color(0, 0, 0);
            };
        }
    }

    #endregion

    #region Stat and Level

    private Dictionary<Stat, int> baseStatPoints = new Dictionary<Stat, int>()
    {
        { Stat.Vit, 0 }, { Stat.Agi, 0 }, { Stat.Arc, 0 },
        { Stat.Str, 0 }, { Stat.Dex, 0 }, { Stat.Int, 0 }
    };

    private Dictionary<Stat, int> statAdds = new Dictionary<Stat, int>()
    {
        { Stat.Vit, 0 }, { Stat.Agi, 0 }, { Stat.Arc, 0 },
        { Stat.Str, 0 }, { Stat.Dex, 0 }, { Stat.Int, 0 }
    };

    private Dictionary<Stat, float> statMults = new Dictionary<Stat, float>()
    {
        { Stat.Vit, 1 }, { Stat.Agi, 1 }, { Stat.Arc, 1 },
        { Stat.Str, 1 }, { Stat.Dex, 1 }, { Stat.Int, 1 }
    };

    private Dictionary<Stat, int> statPoints { get; } = new Dictionary<Stat, int>()
    {
        { Stat.Vit, 0 }, { Stat.Agi, 0 }, { Stat.Arc, 0 },
        { Stat.Str, 0 }, { Stat.Dex, 0 }, { Stat.Int, 0 }
    };

    private int statCap = 150;

    public int level
    {
        get  
        {
            int lvl = 0;
            foreach (int val in baseStatPoints.Values)
                lvl += val;
            return lvl;
        }
    }

    #endregion

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
    /// Returns the base stat point distribution of this character.
    /// </summary>
    public Dictionary<Stat, int> GetBaseStatPoints()
    {
        return baseStatPoints;
    }

    /// <summary>
    /// Returns the base stat point distribution of this character as a list.
    /// </summary>
    public List<int> GetBaseStatPointValues()
    {
        return new List<int>
        {
            baseStatPoints[Stat.Int],
            baseStatPoints[Stat.Arc],
            baseStatPoints[Stat.Dex],
            baseStatPoints[Stat.Agi],
            baseStatPoints[Stat.Str],
            baseStatPoints[Stat.Vit]
        };
    }

    /// <summary>
    /// Set the level of all stats based on the given list.
    /// </summary>
    public void SetBaseStats(List<int> statPoints)
    {
        baseStatPoints[Stat.Int] = statPoints[0];
        baseStatPoints[Stat.Arc] = statPoints[1];
        baseStatPoints[Stat.Dex] = statPoints[2];
        baseStatPoints[Stat.Agi] = statPoints[3];
        baseStatPoints[Stat.Str] = statPoints[4];
        baseStatPoints[Stat.Vit] = statPoints[5];

        UpdateStats();
    }

    /// <summary>
    /// Set all stat levels back to 0;
    /// </summary>
    public void ResetStat()
    {
        foreach (Stat stat in baseStatPoints.Keys)
            baseStatPoints[stat] = 0;
        UpdateStats();
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
    /// Increment the additive boost of the specified stat (base: 0)
    /// </summary>
    public void UpdateStatAdd(Stat stat, int amt)
    {
        statAdds[stat] += amt;
    }

    /// <summary>
    /// Increment the multiplier of the specified stat (base: 1.00)
    /// </summary>
    public void UpdateStatMult(Stat stat, float mult)
    {
        statMults[stat] += mult;
    }

    /// <summary>
    /// Returns the stat point distribution of this character.
    /// </summary>
    public Dictionary<Stat, int> GetStatPoints()
    {
        return statPoints;
    }

    /// <summary>
    /// Returns the stat point distribution of this character as a list.
    /// </summary>
    public List<int> GetStatPointValues()
    {
        return new List<int>
        {
            statPoints[Stat.Int],
            statPoints[Stat.Arc],
            statPoints[Stat.Dex],
            statPoints[Stat.Agi],
            statPoints[Stat.Str],
            statPoints[Stat.Vit]
        };
    }

    /// <summary>
    /// Recalculate the boosted stat levels.
    /// </summary>
    public void UpdateStats()
    {
        foreach (Stat stat in baseStatPoints.Keys)
        {
            statPoints[stat] = (int)(baseStatPoints[stat] * statMults[stat] + statAdds[stat]);
            if (statPoints[stat] > statCap) statPoints[stat] = statCap;
        }
    }

    #endregion
}
