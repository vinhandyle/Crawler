using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Defines the base information of a character.
/// </summary>
public class CharacterProfile : MonoBehaviour
{
    #region Name and Type

    public string charName = "The Player";

    public enum Type
    {
        Normal,
        Miniboss,
        Boss,
        Superboss,
        Player
    };

    [SerializeField] private Type _charType;
    public Type charType { get => _charType; }

    public Color nameColor
    {
        get
        {
            switch (charType)
            {        
                case Type.Miniboss:
                    return new Color(0.75f, 0.75f, 0);

                case Type.Boss:
                    return new Color(0.9f, 0, 0);

                case Type.Superboss:
                    return new Color(0.5f, 0, 0.5f);

                default:
                    return new Color(0, 0, 0);
            };
        }
    }

    public Dictionary<int, int> scanReqs
    {
        get
        {
            switch (charType)
            {
                case Type.Normal:
                    return new Dictionary<int, int>()
                    {
                        { 0, 1 }, { 1, 2 }, { 2, 3 }, { 3, 4 }
                    };

                case Type.Miniboss:
                    return new Dictionary<int, int>()
                    {
                        { 0, 1 }, { 1, 3 }, { 2, 4 }, { 3, 5 }
                    };

                case Type.Boss:
                    return new Dictionary<int, int>()
                    {
                        { 0, 1 }, { 1, 4 }, { 2, 5 }, { 3, 6 }
                    };

                case Type.Superboss:
                    return new Dictionary<int, int>()
                    {
                        { 0, 1 }, { 1, 5 }, { 2, 6 }, { 3, 7 }
                    };

                default:
                    return new Dictionary<int, int>()
                    {
                        { 0, 0 }, { 1, 0 }, { 2, 0 }, { 3, 0 }
                    };
            }
        }
    }

    #endregion

    #region Stat and Level

    private Dictionary<Stats.Stat, int> baseStatPoints = new Dictionary<Stats.Stat, int>()
    {
        { Stats.Stat.Vit, 0 }, { Stats.Stat.Agi, 0 }, { Stats.Stat.Arc, 0 },
        { Stats.Stat.Str, 0 }, { Stats.Stat.Dex, 0 }, { Stats.Stat.Int, 0 }
    };

    private Dictionary<Stats.Stat, int> statAdds = new Dictionary<Stats.Stat, int>()
    {
        { Stats.Stat.Vit, 0 }, { Stats.Stat.Agi, 0 }, { Stats.Stat.Arc, 0 },
        { Stats.Stat.Str, 0 }, { Stats.Stat.Dex, 0 }, { Stats.Stat.Int, 0 }
    };

    private Dictionary<Stats.Stat, float> statMults = new Dictionary<Stats.Stat, float>()
    {
        { Stats.Stat.Vit, 1 }, { Stats.Stat.Agi, 1 }, { Stats.Stat.Arc, 1 },
        { Stats.Stat.Str, 1 }, { Stats.Stat.Dex, 1 }, { Stats.Stat.Int, 1 }
    };

    private Dictionary<Stats.Stat, int> statPoints { get; } = new Dictionary<Stats.Stat, int>()
    {
        { Stats.Stat.Vit, 0 }, { Stats.Stat.Agi, 0 }, { Stats.Stat.Arc, 0 },
        { Stats.Stat.Str, 0 }, { Stats.Stat.Dex, 0 }, { Stats.Stat.Int, 0 }
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

    #region Stat-Dependent Values

    #region VIT

    public int healthBonus = 0;
    public float healthMult = 1;

    #endregion

    #region ARC

    public int manaBonus = 0;
    public float manaMult = 1;
    public int spellSlots
    { 
        get
        { 
            int _slots = statPoints[Stats.Stat.Arc] / 10 + bonusSpellSlots;
            return _slots > 10 ? 10 : _slots;
        } 
    }
    public int bonusSpellSlots = 0;

    #endregion

    #region AGI

    public int staminaBonus = 0;
    public float staminaMult = 1;

    public float speedMult
    {
        get =>  1 + itemSpeedMult + statPoints[Stats.Stat.Agi] / 75f;
    }
    public float itemSpeedMult = 0;

    public float rollTime
    {
        get => 0.25f + statPoints[Stats.Stat.Agi] / 50f;
    }

    #endregion

    #region INT

    public int appraisalLvl
    {
        get => statPoints[Stats.Stat.Int] + appraisalBonus;
    }
    public int appraisalBonus = 0;

    public int analysisLvl = 0;
    public bool SectionIsVisible(int section, CharacterProfile observer, bool hidden = false)
    {
        int reqInc = hidden ? 1 : 0;
        return observer.analysisLvl >= scanReqs[section] + reqInc;
    }

    #endregion

    #region DEX

    public int pickpocketLvl
    {
        get => (int)(statPoints[Stats.Stat.Dex] * pickpocketMult) + pickpocketBonus;
    } 
    public int pickpocketBonus = 0;
    public float pickpocketMult = 1;

    public int lockpickingLvl
    {
        get  => (int)(statPoints[Stats.Stat.Dex] * lockpickingMult) + lockpickingBonus;
    }
    public int lockpickingBonus = 0;
    public float lockpickingMult = 1;

    #endregion

    #region STR

    public float equipLoad
    {
        get => statPoints[Stats.Stat.Str] * equipLoadMult;
    }
    public float equipLoadMult = 1;

    public float staminaUseReduction
    {
        get => statPoints[Stats.Stat.Str];
    }

    #endregion

    #endregion

    #region Base

    /// <summary>
    /// Returns the base stat point distribution of this character.
    /// </summary>
    public Dictionary<Stats.Stat, int> GetBaseStatPoints()
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
            baseStatPoints[Stats.Stat.Int],
            baseStatPoints[Stats.Stat.Arc],
            baseStatPoints[Stats.Stat.Dex],
            baseStatPoints[Stats.Stat.Agi],
            baseStatPoints[Stats.Stat.Str],
            baseStatPoints[Stats.Stat.Vit]
        };
    }

    /// <summary>
    /// Set the level of all stats based on the given list.
    /// </summary>
    public void SetBaseStats(List<int> statPoints)
    {
        baseStatPoints[Stats.Stat.Int] = statPoints[0];
        baseStatPoints[Stats.Stat.Arc] = statPoints[1];
        baseStatPoints[Stats.Stat.Dex] = statPoints[2];
        baseStatPoints[Stats.Stat.Agi] = statPoints[3];
        baseStatPoints[Stats.Stat.Str] = statPoints[4];
        baseStatPoints[Stats.Stat.Vit] = statPoints[5];

        UpdateStats();
    }

    /// <summary>
    /// Set all stat levels back to 0;
    /// </summary>
    public void ResetStat()
    {
        foreach (Stats.Stat stat in baseStatPoints.Keys)
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
    public void UpdateStatAdd(Stats.Stat stat, int amt)
    {
        statAdds[stat] += amt;
    }

    /// <summary>
    /// Increment the multiplier of the specified stat (base: 1.00)
    /// </summary>
    public void UpdateStatMult(Stats.Stat stat, float mult)
    {
        statMults[stat] += mult;
    }

    /// <summary>
    /// Returns the stat point distribution of this character.
    /// </summary>
    public Dictionary<Stats.Stat, int> GetStatPoints()
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
            statPoints[Stats.Stat.Int],
            statPoints[Stats.Stat.Arc],
            statPoints[Stats.Stat.Dex],
            statPoints[Stats.Stat.Agi],
            statPoints[Stats.Stat.Str],
            statPoints[Stats.Stat.Vit]
        };
    }

    /// <summary>
    /// Recalculate the boosted stat levels.
    /// </summary>
    public void UpdateStats()
    {
        foreach (Stats.Stat stat in baseStatPoints.Keys)
        {
            statPoints[stat] = (int)(baseStatPoints[stat] * statMults[stat] + statAdds[stat]);
            if (statPoints[stat] > statCap) statPoints[stat] = statCap;
        }
    }

    #endregion
}
