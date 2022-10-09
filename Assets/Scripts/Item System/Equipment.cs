using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Stats;

/// <summary>
/// Defines a character's loadout.
/// </summary>
public class Equipment : MonoBehaviour
{
    [SerializeField] private CharacterProfile profile;

    public List<Effect> effects;

    public Armor helmet;
    public Armor chestplate;
    public Armor leggings;
    public Armor boots;
    public List<Accessory> accessories;
    public List<Spell> spells;

    public Weapon[] weapons = new Weapon[2];
    public Ammo[] ammos = new Ammo[2];
    public Consumable[] consumables  = new Consumable[10];

    /// <summary>
    /// Returns the profile of the associated character.
    /// </summary>
    public CharacterProfile GetCharacter()
    {
        return profile;
    }

    /// <summary>
    /// Returns the stat point distribution of this character.
    /// </summary>
    public Dictionary<Stat, int> GetStatPoints()
    {
        return profile.GetStatPoints();
    }

    /// <summary>
    /// Returns the stat point distribution of this character as a list.
    /// </summary>
    public List<int> GetStatPointValues()
    {
        return profile.GetStatPointValues();
    }

    public int GetMaxGaugeValue(Stats.Gauge gauge)
    { 
        switch (gauge)
        {
            case Gauge.HP:
                return 0;

            case Gauge.MP:
                return 0;

            case Gauge.SP:
                return 0;

            default:
                return 0;
        }
    }

    public int GetGaugeValue(Stats.Gauge gauge)
    {
        switch (gauge)
        {
            case Gauge.HP:
                return 0;

            case Gauge.MP:
                return 0;

            case Gauge.SP:
                return 0;

            default:
                return 0;
        }
    }
}
