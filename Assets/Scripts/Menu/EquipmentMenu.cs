using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Defines the equipment UI.
/// </summary>
public class EquipmentMenu : CharacterMenu
{
    [SerializeField] private Equipment equipment;

    [Header("Stats")]
    [SerializeField] private RadarChart rc;
    [SerializeField] private Text displayName;
    [SerializeField] private Text displayStats;
    [SerializeField] private Text displayEffects;

    [Header("Gear")]
    [SerializeField] private Button helmetSlot;
    [SerializeField] private Button chestplateSlot;
    [SerializeField] private Button leggingsSlot;
    [SerializeField] private Button bootsSlot;
    [SerializeField] private List<Button> weaponSlots;
    [SerializeField] private List<Button> ammoSlots;
    [SerializeField] private List<Button> accessorySlots;
    [SerializeField] private List<Button> consumableSlots;
    [SerializeField] private List<Button> spellSlots;

    [Header("Menu")]
    [SerializeField] private List<Scrollbar> scrollbars;
    [SerializeField] private LevelUpMenu lum;
    [SerializeField] private EquipMenu em;

    [Header("Analysis")]
    [SerializeField] private CharacterProfile player;
    [SerializeField] private CharacterProfile target;
    [SerializeField] private GameObject spellSection;
    [SerializeField] private List<GameObject> gearSection;

    protected override void Awake()
    {
        base.Awake();
        rc?.SetClickBehavior(lum);

        allowSwitchEvent = false;
        OnSwitch += () => { allowSwitchEvent = true; };
    }

    public override void Open()
    {
        displayStats.text = "";
        base.Open();

        foreach (Scrollbar scrollbar in scrollbars) scrollbar.value = 1;
    }

    public override void Close()
    {
        lum?.Close();
        em?.Close();
        base.Close();

        allowSwitchEvent = false;
    }

    #region Menu Setup

    /// <summary>
    /// Set the target data to the specified equipment.
    /// </summary>
    public void Set(Equipment equipment)
    {
        this.equipment = equipment;
        target = equipment?.GetComponent<CharacterProfile>();
    }

    /// <summary>
    /// Set the visibility of each section of the equipment menu based on
    /// the player's analysis level and the section's requirement.
    /// </summary>
    private void SetSectionVisibility()
    {
        rc.gameObject.SetActive(target.SectionIsVisible(2, player));
        displayStats.gameObject.SetActive(target.SectionIsVisible(0, player));
        displayEffects.gameObject.SetActive(target.SectionIsVisible(2, player));
        spellSection.SetActive(target.SectionIsVisible(3, player));
        gearSection.ForEach(section => section.SetActive(target.SectionIsVisible(1, player)));
    }

    /// <summary>
    /// Load the name and level of the character.
    /// </summary>
    private void LoadName()
    {
        displayName.text = string.Format(
            "<color={2}>{0}</color>\n{1}", 
            new MenuHelper().StringWindow(equipment.GetCharacter().charName, 10), 
            target.SectionIsVisible(1, player) ? "Lvl " + (equipment.GetCharacter().level + 1) : "",
            "#" + ColorUtility.ToHtmlStringRGBA(equipment.GetCharacter().nameColor)
            );
    }

    /// <summary>
    /// Load the stat section (total damage and defense)
    /// of the equipment menu.
    /// </summary>
    private void LoadStats()
    { 
        Dictionary<Stats.Stat, int> stats = equipment.GetStatPoints();

        List<Item> equippedItems = new List<Item>()
        {
            equipment.helmet, equipment.chestplate, 
            equipment.leggings, equipment.boots
        };
        equippedItems.AddRange(equipment.accessories);
        equippedItems.AddRange(equipment.weapons);

        string gaugeText = "";
        string dmgText = "";
        string defText = "";

        foreach (KeyValuePair<Stats.Gauge, Color> kvp in new Stats().gaugeColor)
        {
            gaugeText += string.Format(
                "<color={3}>{0}: {1}/{2}</color>\n",
                kvp.Key,
                equipment.GetGaugeValue(kvp.Key),
                equipment.GetMaxGaugeValue(kvp.Key),
                "#" + ColorUtility.ToHtmlStringRGBA(kvp.Value)
                );
        }

        foreach (KeyValuePair<Stats.Damage, Color> kvp in new Stats().dmgColor)
        {
            int totalDmg = equippedItems.Where(i => i != null && i.GetItemClass() == Weapon.GetStaticItemClass())
                                   .Sum(
                i => i.GetStats(stats[Stats.Stat.Str], stats[Stats.Stat.Dex], stats[Stats.Stat.Int])[kvp.Key]
                );

            int totalDef = equippedItems.Where(i => i != null && i.GetItemClass() != Weapon.GetStaticItemClass())
                                   .Sum(
                i => i.GetStats(stats[Stats.Stat.Str], stats[Stats.Stat.Dex], stats[Stats.Stat.Int])[kvp.Key]
                );

            dmgText += string.Format
                (
                    "<color={2}>{0}</color> Dmg:\t<color=black>{1}</color>\n",
                    kvp.Key.ToString().Substring(0, 4),
                    totalDmg,
                    "#" + ColorUtility.ToHtmlStringRGBA(kvp.Value)
                );

            defText += string.Format
                (
                    "<color={2}>{0}</color> Def:\t<color=black>{1}</color>\n",
                    kvp.Key.ToString().Substring(0, 4),
                    totalDef,
                    "#" + ColorUtility.ToHtmlStringRGBA(kvp.Value)
                );           
        }

        displayStats.text = gaugeText + "\n" + dmgText + "\n" + defText;
    }

    /// <summary>
    /// Load the character's non-gear effects.
    /// </summary>
    private void LoadEffects()
    {
        displayEffects.text = new MenuHelper().StringWindow(
            string.Join("\n", equipment.effects.Select(e => e.GetDescripton())), 25);
    }

    #endregion

    #region Button Setup

    /// <summary>
    /// Sets the sprite of the button to the specified one, if it exists.
    /// </summary>
    private void SetItemSprite(Image img, Sprite sprite, string defaultPath = "Graphics/Items/Template")
    {
        img.sprite = (sprite != null) ? sprite : Resources.Load<Sprite>(defaultPath);
    }

    /// <summary>
    /// Load the item information into the button. 
    /// Left-click to load item into info menu.
    /// Right-click to equip a another item.
    /// </summary>
    public void SetSlotButton(Button btn, Item item, string defaultPath, int index = 0)
    {
        string[] path = defaultPath.Split('/');
        SetItemSprite(btn.image, item?.sprite, defaultPath);

        btn.onClick.RemoveAllListeners();
        Destroy(btn.GetComponent<RightClick>());

        btn.onClick.AddListener(() => { infoMenu.SetItem(item, equipment); });
        btn.gameObject.AddComponent<RightClick>().onRightClick += () =>
        {
            infoMenu.SetItem(item, equipment);
            em.Trigger(item, path[path.Length - 1].Split(' ')[0]);
            em.OnConfirm += (Item i, string type) => { SetSlotItem(i, type, index); };
        };
    }

    /// <summary>
    /// Equip the given item into the slot of the given type (with the specified index if applicable).
    /// </summary>
    public void SetSlotItem(Item item, string type, int index = 0)
    {
        switch (type)
        {
            case "Helmet":
                equipment.helmet = (Armor)item;
                break;

            case "Chestplate":
                equipment.chestplate = (Armor)item;
                break;

            case "Leggings":
                equipment.leggings = (Armor)item;
                break;

            case "Boots":
                equipment.boots = (Armor)item;
                break;

            case "Weapon":
                equipment.weapons[index] = (Weapon)item;
                break;

            case "Ammo":
                equipment.ammos[index] = (Ammo)item;
                break;

            case "Accessory":
                equipment.accessories[index] = (Accessory)item;
                break;

            case "Consumable":
                equipment.consumables[index] = (Consumable)item;
                break;

            case "Spell":
                equipment.spells[index] = (Spell)item;
                break;
        }
    }

    #endregion

    public override void Load()
    {
        base.Load();

        // Do not load if target has no equipment
        if (!equipment) return;

        rc.SetValues(equipment.GetStatPointValues());
        rc.GenerateChart();

        LoadName();
        LoadStats();
        LoadEffects();

        SetSectionVisibility();

        #region Gear Buttons

        // Clear accesory slots
        for (int i = 1; i < accessorySlots.Count; i += 0)
        {
            Button btn = accessorySlots[i];
            accessorySlots.Remove(btn);
            Destroy(btn.gameObject);
        }

        // Clear spell slots
        foreach (Button btn in spellSlots) btn.gameObject.SetActive(false);

        // Generate accessory slots
        for (int i = 1; i < equipment.accessories.Count; ++i)
        {
            Button btn = Instantiate(accessorySlots[0], accessorySlots[0].transform.parent);
            accessorySlots.Add(btn);
        }

        // Load gear
        SetSlotButton(helmetSlot, equipment.helmet, "Graphics/Items/Equipment Slots/Helmet Empty");
        SetSlotButton(chestplateSlot, equipment.chestplate, "Graphics/Items/Equipment Slots/Chestplate Empty");
        SetSlotButton(leggingsSlot, equipment.leggings, "Graphics/Items/Equipment Slots/Leggings Empty");
        SetSlotButton(bootsSlot, equipment.boots, "Graphics/Items/Equipment Slots/Boots Empty");

        for (int i = 0; i < 2; ++i)
        {
            SetSlotButton(weaponSlots[i], equipment.weapons[i], "Graphics/Items/Equipment Slots/Weapon Empty", i);
            SetSlotButton(ammoSlots[i], equipment.ammos[i], "Graphics/Items/Equipment Slots/Ammo Empty", i);
        }

        // Load accessories
        for (int i = 0; i < equipment.accessories.Count; ++i)
        {
            SetSlotButton(accessorySlots[i], equipment.accessories[i], "Graphics/Items/Equipment Slots/Accessory Empty", i);
        }

        // Load consumables
        for (int i = 0; i < equipment.consumables.Length; ++i)
        {
            SetSlotButton(consumableSlots[i], equipment.consumables[i], "Graphics/Items/Equipment Slots/Consumable Empty", i);
        }

        // Load spells
        for (int i = 0; i < equipment.spells.Count; ++i)
        {
            spellSlots[i].gameObject.SetActive(true);
            SetSlotButton(spellSlots[i], equipment.spells[i], "Graphics/Items/Equipment Slots/Spell Empty", i);
        }

        #endregion
    }
}
