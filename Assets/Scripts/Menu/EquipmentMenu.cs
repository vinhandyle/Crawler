using System.Collections;
using System.Collections.Generic;
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

    [Header("Gear")]
    [SerializeField] private Button helmetSlot;
    [SerializeField] private Button chestplateSlot;
    [SerializeField] private Button leggingsSlot;
    [SerializeField] private Button bootsSlot;
    [SerializeField] private List<Button> weaponSlots;
    [SerializeField] private List<Button> ammoSlots;
    [SerializeField] private List<Button> accessorySlots;
    [SerializeField] private List<Button> consumableSlots;
    [SerializeField] private List<Button> spellTechSlots;

    [Header("Menu")]
    [SerializeField] private List<Scrollbar> scrollbars;
    [SerializeField] private EquipMenu em;

    public override void Open()
    {
        base.Open();

        foreach (Scrollbar scrollbar in scrollbars) scrollbar.value = 1;
    }

    public override void Close()
    {
        em?.Close();
        base.Close();
    }

    #region Menu Setup

    /// <summary>
    /// Set the target data to the specified equipment.
    /// </summary>
    public void Set(Equipment equipment)
    {
        this.equipment = equipment;
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
                equipment.helmet = item?.GetComponent<Armor>();
                break;

            case "Chestplate":
                equipment.chestplate = item?.GetComponent<Armor>();
                break;

            case "Leggings":
                equipment.leggings = item?.GetComponent<Armor>();
                break;

            case "Boots":
                equipment.boots = item?.GetComponent<Armor>();
                break;

            case "Weapon":
                equipment.weapons[index] = item?.GetComponent<Weapon>();
                break;

            case "Ammo":
                equipment.ammos[index] = item?.GetComponent<Ammo>();
                break;

            case "Accessory":
                equipment.accessories[index] = item?.GetComponent<Accessory>();
                break;

            case "Consumable":
                equipment.consumables[index] = item?.GetComponent<Consumable>();
                break;
        }
    }

    #endregion

    public override void Load()
    {
        base.Load();

        rc.SetValues(equipment.GetStatPointValues());
        rc.GenerateChart();

        #region Gear Buttons

        // Clear accesory slots
        for (int i = 1; i < accessorySlots.Count; i += 0)
        {
            Button btn = accessorySlots[i];
            accessorySlots.Remove(btn);
            Destroy(btn.gameObject);
        }

        // Clear spell/tech slots
        foreach (Button btn in spellTechSlots) btn.gameObject.SetActive(false);

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

        // Load spells/techniques
        for (int i = 0; i < equipment.spellTechs.Count; ++i)
        {
            spellTechSlots[i].gameObject.SetActive(true);
            SetSlotButton(spellTechSlots[i], equipment.spellTechs[i], "Graphics/Items/Equipment Slots/Spell_Tech Empty", i);
        }

        #endregion
    }
}
