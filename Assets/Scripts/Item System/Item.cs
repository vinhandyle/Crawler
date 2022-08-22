using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Stats;

/// <summary>
/// Base class for all items.
/// </summary>
public abstract class Item : MonoBehaviour
{
    public int itemTypeID { get;  protected set; }
    public int value { get; protected set; }
    public Sprite sprite { get => _sprite; }
    [SerializeField] protected Sprite _sprite;

    public bool statsSet;

    [Header("Stackable Item")]
    public bool stackable;
    public int quantity { get => _quantity; set { _quantity = value; } }
    [SerializeField] protected int _quantity;

    [Header("Item Info")]
    protected Dictionary<int, string> names = new Dictionary<int, string>();
    protected Dictionary<int, string> descriptions = new Dictionary<int, string>();
    protected Dictionary<Stat, int> reqs = new Dictionary<Stat, int>()
    {
        { Stat.Str, 0 }, { Stat.Dex, 0 }, { Stat.Int, 0 }
    };
    protected Dictionary<Gauge, int> useCosts = new Dictionary<Gauge, int>()
    {
        { Gauge.HP, 0 }, { Gauge.MP, 0 }, { Gauge.SP, 0 }
    };
    protected Dictionary<Damage, int> baseStats;
    protected Dictionary<int[], Effect> effects = new Dictionary<int[], Effect>();

    /// <summary>
    /// Create a new instance of the item prefab. 
    /// Necessary for having multiple of the same item without conflict.
    /// </summary>
    public Item CloneFromPrefab()
    {
        Debug.Log(this);

        Transform parent = (transform.parent != null) ? transform.parent : GameObject.Find("Item/Spell/Tech Dump").transform;
        Item item = Instantiate(this, parent);
        item.SetBaseInfo();
        return item;
    }

    /// <summary>
    /// Set the item's base information.
    /// </summary>
    public virtual void SetBaseInfo()
    {
        statsSet = true;
    }

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
    public abstract Dictionary<Damage, int> GetStats(int str, int dex, int @int);

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
