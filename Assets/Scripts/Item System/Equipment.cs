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

    public Armor helmet;
    public Armor chestplate;
    public Armor leggings;
    public Armor boots;
    public List<Accessory> accessories;
    public List<Item> spellTechs;

    public Weapon[] weapons = new Weapon[2];
    public Ammo[] ammos = new Ammo[2];
    public Consumable[] consumables  = new Consumable[10];

    /// <summary>
    /// Returns the stat point distribution of this character.
    /// </summary>
    public Dictionary<Stat, int> GetStatPoints()
    {
        return profile.statPoints;
    }

    public List<int> GetStatPointValues()
    {
        return new List<int>
        {
            profile.statPoints[Stat.Int],
            profile.statPoints[Stat.Arc],
            profile.statPoints[Stat.Dex],
            profile.statPoints[Stat.Agi],
            profile.statPoints[Stat.Str],
            profile.statPoints[Stat.Vit]
        };
    }
}
