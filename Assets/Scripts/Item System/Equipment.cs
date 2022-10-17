using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Defines a character's loadout.
/// </summary>
public class Equipment : MonoBehaviour
{
    [SerializeField] private CharacterProfile profile;
    [SerializeField] private Inventory inventory;
    [SerializeField] private Gauge gauges;

    public Armor helmet;
    public Armor chestplate;
    public Armor leggings;
    public Armor boots;
    public List<Accessory> accessories = new List<Accessory>();
    public List<Spell> spells = new List<Spell>();
    public List<Effect> effects = new List<Effect>();

    public Weapon[] weapons = new Weapon[2];
    public Ammo[] ammos = new Ammo[2];
    public Consumable[] consumables  = new Consumable[10];

    private void Awake()
    {
        profile = GetComponent<CharacterProfile>();
        inventory = GetComponent<Inventory>();
        gauges = GetComponent<Gauge>();
    }

    private void Update()
    {
        if (profile?.charType == CharacterProfile.Type.Player)
        {
            AdjustAccessorySlots();
            if (AdjustSpellSlots())
                GameObject.Find("Character Menu").GetComponentInChildren<EquipmentMenu>()?.Load();
        }
    }

    public void SetDefaults(Character character)
    {
        profile = character.profile;
        inventory = character.inventory;
    }

    #region Character Stats

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
    public Dictionary<Stats.Stat, int> GetStatPoints()
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

    /// <summary>
    /// Returns the max value of the specified gauge.
    /// </summary>
    public int GetMaxGaugeValue(Stats.Gauge gauge)
    { 
        switch (gauge)
        {
            case Stats.Gauge.HP:
                return gauges.maxHealth;

            case Stats.Gauge.MP:
                return gauges.maxMana;

            case Stats.Gauge.SP:
                return gauges.maxStamina;

            default:
                return 0;
        }
    }

    /// <summary>
    /// Returns the current value of the specified gauge.
    /// </summary>
    public int GetGaugeValue(Stats.Gauge gauge)
    {
        switch (gauge)
        {
            case Stats.Gauge.HP:
                return gauges.health;

            case Stats.Gauge.MP:
                return gauges.mana;

            case Stats.Gauge.SP:
                return gauges.stamina;

            default:
                return 0;
        }
    }

    #endregion

    #region Equipment Menu

    /// <summary>
    /// Adjust the accessory slots when there is an increase in size.
    /// </summary>
    private void AdjustAccessorySlots()
    {
        if (accessories.Count < inventory.accessorySlots)
        {
            accessories.Add(null);
        }
    }
    
    /// <summary>
    /// Adjust the spell slots when there is a change in the max size.
    /// For size reduction, unequip the excess spells first.
    /// </summary>
    private bool AdjustSpellSlots()
    {
        int n = spells.Count;

        if (spells.Count < profile.spellSlots)
        {
            spells.Add(null);
        }
        else if (spells.Count > profile.spellSlots)
        {
            for (int i = profile.spellSlots; i < spells.Count; ++i)
            {
                profile.GetComponent<Inventory>().AddItem(spells[i]);
            }
            spells = spells.GetRange(0, profile.spellSlots);
        }

        return n != spells.Count;
    }

    public void FullEquipWeapon<T>()
    {
        List<Item> weapons = inventory.GetAllItems().Where(i => i.GetType() == typeof(T)).ToList();
        for (int i = 0; i < weapons.Count; ++i)
        {
            this.weapons[i] = (Weapon)weapons[i];
            inventory.RemoveItem(weapons[i]);
        }
    }

    #endregion
}
