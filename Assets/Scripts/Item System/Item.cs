using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Stats;

/// <summary>
/// Base class for all items.
/// </summary>
public abstract class Item
{
    public int itemTypeID { get;  protected set; }
    public int value { get; protected set; }
    public Sprite sprite;
    protected string spritePath = "Graphics/Items/";

    [Header("Stackable Item")]
    public bool stackable;
    public int quantity;

    [Header("Item Info")]
    public bool lootable = true;
    public bool stashable = true;
    public bool buyable = true;
    public bool sellable = true;
    public bool stealable = true;
    protected Dictionary<int, string> names = new Dictionary<int, string>();
    protected Dictionary<int, string> descriptions = new Dictionary<int, string>();
    protected Dictionary<Stat, int> reqs;
    protected Dictionary<Gauge, int> useCosts;
    protected Dictionary<Damage, int> baseStats;
    protected Dictionary<int[], Effect> effects = new Dictionary<int[], Effect>();

    /// <summary>
    /// Get the name of the item's class.
    /// </summary>
    public abstract string GetItemClass();

    /// <summary>
    /// Return a copy of this item.
    /// </summary>
    public Item Clone()
    {
        return (Item)MemberwiseClone();
    }

    /// <summary>
    /// Returns the item's sprite.
    /// </summary>
    protected Sprite GetSprite(string path)
    {
        return Resources.Load<Sprite>(spritePath + path);
    }

    /// <summary>
    /// Set the item's stat requirements.
    /// </summary>
    protected void SetRequirements(int str, int dex, int @int)
    {
        reqs = new Dictionary<Stat, int>()
        {
            { Stat.Str, str }, { Stat.Dex, dex }, { Stat.Int, @int }
        };
    }

    /// <summary>
    /// Set the item's use costs.
    /// </summary>
    protected void SetUseCosts(int hp, int mp, int sp)
    {
        useCosts = new Dictionary<Gauge, int>()
        {
            { Gauge.HP, hp }, { Gauge.MP, mp }, { Gauge.SP, sp }
        };
    }

    #region Meta Data

    public string[] GetItemType()
    {
        // [00][00][00][000]
        // [category][type][subtype][item]

        int _itemTypeID = itemTypeID / 1000;
        int subType = _itemTypeID % 100;
        int type = _itemTypeID / 100 % 100;
        int category = _itemTypeID / 100 / 100 % 100;

        string[] itemType = new string[2];

        switch (category)
        {
            // Consumable
            case 0:
                break;

            // Material
            case 1:
                break;

            // Key Item
            case 2:
                break;

            // Spell
            case 3:
                break;

            // Technique
            case 4:
                break;

            // Weapon
            case 5:
                break;

            // Armor
            case 6:
                switch (type)
                {
                    case 0:
                        itemType[0] = "Helmet";
                        break;

                    case 1:
                        itemType[1] = "Chestplate";
                        break;

                    case 2:
                        itemType[2] = "Leggings";
                        break;

                    case 3:
                        itemType[3] = "Boots";
                        break;
                }
                break;

            // Accessory
            case 7:
                break;

            // Ammo
            case 8:
                break;
        }

        return itemType;

    }

    #endregion

    #region Display

    /// <summary>
    /// Returns the name of the item based on the given appraisal level.
    /// </summary>
    public string GetName(int appraisalLvl)
    {
        if (names.Count == 0) return "";
        return names[names.Keys.Where(req => req <= appraisalLvl).OrderByDescending(x => x).First()];
    }

    /// <summary>
    /// Returns the description of the item based on the given appraisal level.
    /// </summary>
    public string GetDescription(int appraisalLvl)
    {
        if (descriptions.Count == 0) return "";
        return descriptions[descriptions.Keys.Where(req => req <= appraisalLvl).OrderByDescending(x => x).First()];
    }

    /// <summary>
    /// Returns the item's requirements (for displaying).
    /// </summary>
    public Dictionary<Stat, int> GetRequirements()
    {
        return reqs;
    }

    /// <summary>
    /// Returns the item's use costs (for displaying).
    /// </summary>
    public Dictionary<Gauge, int> GetUseCosts()
    {
        return useCosts;
    }

    /// <summary>
    /// Returns the base stat distribution of this weapon.
    /// </summary>
    public virtual Dictionary<Damage, int> GetBaseStats()
    {
        return baseStats;
    }

    /// <summary>
    /// Returns the item's stats based on the given skill levels.
    /// </summary>
    public virtual Dictionary<Damage, int> GetStats(int str, int dex, int @int)
    {
        return GetBaseStats();
    }

    /// <summary>
    /// Returns a list of effects available from this item based on the given dexterity and intelligence.
    /// </summary>
    public List<Effect> GetEffects(int dex, int @int)
    {
        List<Effect> effects = new List<Effect>();
        foreach (KeyValuePair<int[], Effect> kvp in this.effects)
        {
            if (kvp.Key[0] <= dex && kvp.Key[1] <= @int) effects.Add(kvp.Value);
        }
        return effects;
    }

    /// <summary>
    /// Returns the given stat scaled based on item's innate scaling and the given skill levels.
    /// By default, scaling is linear with no caps.
    /// </summary>
    protected virtual int GetScaledStat(Dictionary<Stat, int> reqs, int[] lvls, float[] scaling)
    {
        int stat = 0;

        if (lvls[0] >= reqs[Stat.Str])
        {

        }

        if (lvls[1] >= reqs[Stat.Dex])
        {

        }

        if (lvls[2] >= reqs[Stat.Int])
        {

        }

        for (int i = 0; i < 3; ++i)
        {
            stat += (int)(lvls[i] * scaling[i]);
        }
        return stat;
    }

    #endregion
}
