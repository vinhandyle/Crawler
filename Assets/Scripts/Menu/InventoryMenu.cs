using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Defines the inventory UI.
/// </summary>
public class InventoryMenu : CharacterMenu
{
    [SerializeField] private Inventory inventory;

    [Header("Menu")]
    [SerializeField] private Mode mode;
    [SerializeField] private Text displayMode;
    [SerializeField] private List<Button> modeBtns;
    [SerializeField] private GameObject itemRow;
    [SerializeField] private List<GameObject> copyRows;
    [SerializeField] private List<Scrollbar> scrollbars;

    [Header("Transfer")]
    public string transferType;
    [SerializeField] private Text displayMoney;
    [SerializeField] private TransferMenu tm;

    [Header("Use Consumable")]
    [SerializeField] private UseMenu um;

    private enum Mode
    {
        All,
        Consumable,
        Material,
        KeyItem,
        Weapon,
        Ammo,
        Armor,
        Accessory,
        Spell,
        Technique
    }

    protected override void Awake()
    {
        base.Awake();
        for (int i = 0; i < modeBtns.Count; ++i)
        {
            int j = i;
            modeBtns[i].onClick.AddListener(() => { SelectItemType(j); });
        }
    }

    public override void Open()
    {
        base.Open();
        foreach (Scrollbar scrollbar in scrollbars)
        {
            scrollbar.value = (scrollbar.direction == Scrollbar.Direction.LeftToRight) ? 0 : 1; 
        }
        tm.Close();
        um.Close();
    }

    public override void Close()
    {
        base.Close();
        tm.Close();
        um.Close();
        transferType = "";
    }

    #region Menu Setup

    /// <summary>
    /// Set the target data to the specified inventory.
    /// </summary>
    public void Set(Inventory inventory)
    {
        this.inventory = inventory;
        SetDefaultSwitchable();
    }

    /// <summary>
    /// Restores the switchable status of this menu to the one specified by the inventory.
    /// </summary>
    public void SetDefaultSwitchable()
    {
        SetSwitchable(!inventory.standalone);
    }

    #endregion

    #region Button Setup
    
    /// <summary>
    /// Set the mode of the inventory menu to the specified category.
    /// </summary>
    private void SelectItemType(int category)
    {
        switch (category)
        {
            case 0:
                mode = Mode.All;
                break;

            case 1:
                mode = Mode.Consumable;
                break;

            case 2:
                mode = Mode.Material;
                break;

            case 3:
                mode = Mode.KeyItem;
                break;

            case 4:
                mode = Mode.Spell;
                break;

            case 5:
                mode = Mode.Technique;
                break;

            case 6:
                mode = Mode.Weapon;

                break;

            case 7:
                mode = Mode.Ammo;
                break;

            case 8:
                mode = Mode.Armor;
                break;

            case 9:
                mode = Mode.Accessory;
                break;
        }
        tm.Close();
        um.Close();
        Load();
    }

    /// <summary>
    /// Sets the sprite of the button to the specified one, if it exists.
    /// </summary>
    private void SetItemSprite(Image img, Sprite sprite)
    {
        img.sprite = (sprite != null) ? sprite : Resources.Load<Sprite>("Graphics/Items/Template");
    }

    /// <summary>
    /// Load the item information into the button along with functionality based on the current transfer type.
    /// </summary>
    private void SetItemButton(Transform btn, Item item)
    {
        // Button
        btn.gameObject.SetActive(true);
        SetItemSprite(btn.GetComponent<Image>(), item.sprite);

        // Left-click
        btn.GetComponent<Button>().onClick.RemoveAllListeners();
        btn.GetComponent<Button>().onClick.AddListener
            (
                () =>
                {
                    tm.Close();
                    um.Close();
                    infoMenu.SetItem(item);
                }
            );

        // Right-click
        Destroy(btn.GetComponent<RightClick>());
        btn.gameObject.AddComponent<RightClick>().onRightClick += () =>
        {
            if (transferType != "")
            {
                infoMenu.SetItem(item);
                tm.Trigger(this, inventory, connMenus[0].GetComponent<InventoryMenu>().inventory, item, transferType);
            }
            else
            {
                if (item.GetItemClass() == Consumable.GetStaticItemClass())
                {
                    infoMenu.SetItem(item);
                    um.Trigger(item);
                }
            }
        };
    }

    #endregion

    public override void Load()
    {
        base.Load();
        displayMoney.text = string.Format("{0} G", inventory.coins);

        // Display current mode
        switch (mode)
        {
            case Mode.All:
                displayMode.text = "All Items";
                break;

            case Mode.KeyItem:
                displayMode.text = "Key Items";
                break;

            case Mode.Ammo:
                displayMode.text = "Ammunitions";
                break;

            case Mode.Accessory:
                displayMode.text = "Accessories";
                break;

            default:
                displayMode.text = mode.ToString() + "s";
                break;
        }

        #region Item Buttons

        // Get items to display
        List<Item> items = inventory.GetAllItems()
                                    .FindAll
                                    (
                                        i => mode == Mode.All || mode.ToString() == i.GetItemClass()
                                    )
                                    .FindAll
                                    (
                                        i => (transferType == "") || 
                                        (transferType == "Loot" && i.lootable) || 
                                        (transferType == "Stash" && i.stashable) ||
                                        (transferType == "Buy" && i.buyable) ||
                                        (transferType == "Sell" && i.sellable) ||
                                        (transferType == "Steal" && i.stealable)
                                    )
                                    .OrderBy(item => item.itemTypeID)
                                    .ToList();

        // Clear item buttons
        foreach (GameObject copyRow in copyRows) Destroy(copyRow);
        copyRows.Clear();

        // Generate item buttons
        int a = items.Count;
        int b = itemRow.transform.childCount;
        if (a > b)
        {
            int rows = (a - b) / b + (a % b == 0 ? 0 : 1);
            for (int i = 0; i < rows; ++i)
            {
                GameObject copyRow = Instantiate(itemRow, itemRow.transform.parent);
                copyRows.Add(copyRow);
            }
        }

        // Load item button functionality
        for (int i = 0; i < a; ++i)
        {
            int n = i;           

            if (i < itemRow.transform.childCount)
            {
                SetItemButton(itemRow.transform.GetChild(i), items[n]);                
            }
            else
            {
                int row = (i - b) / b;
                int col = i % b;
                SetItemButton(copyRows[row].transform.GetChild(col), items[n]);                
            }
        }

        // Unload excess buttons
        if (a == 0 || a % b != 0)
        {
            GameObject row = (a / b == 0) ? itemRow : copyRows[a / b - 1];
            for (int i = a % b; i < b; ++i)
            {
                row.transform.GetChild(i).gameObject.SetActive(false);
            }
        }

        #endregion
    }
}