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
        btn.GetComponent<Button>().onClick.AddListener
            (
                () =>
                {
                    tm.Close();
                    infoMenu.SetItem(item);

                    /*
                    switch (transferType)
                    {
                        case "Buy":
                            infoMenu.SetExtra(InfoMenu.Type.Buy, item.value);
                            break;

                        case "Sell":
                            infoMenu.SetExtra(InfoMenu.Type.Sell, item.value);
                            break;

                        case "Steal":
                            infoMenu.SetExtra(InfoMenu.Type.Steal, item.value);
                            break;
                    }
                    */
                }
            );

        // Right-click
        btn.gameObject.AddComponent<RightClick>().onRightClick += () =>
        {
            if (transferType != "")
            {
                tm.Trigger(this, inventory, connMenus[0].GetComponent<InventoryMenu>().inventory, item, transferType);
                connMenus[0].GetComponent<InventoryMenu>().Load();
                Load();
            }
            else
            {
                if (item.GetComponent<Consumable>() != null)
                {
                    // Open advanced menu w/ use button
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

        // Get items to display
        List<Item> items = inventory.GetAllItems()
                                    .FindAll
                                    (
                                        i => (mode == Mode.All) ||
                                            (mode == Mode.Consumable && i.GetComponent<Consumable>()) ||
                                            (mode == Mode.Material && i.GetComponent<Material>()) ||
                                            (mode == Mode.KeyItem && i.GetComponent<KeyItem>()) ||
                                            (mode == Mode.Spell && i.GetComponent<Spell>()) ||
                                            (mode == Mode.Technique && i.GetComponent<Technique>()) ||
                                            (mode == Mode.Weapon && i.GetComponent<Weapon>()) ||
                                            (mode == Mode.Ammo && i.GetComponent<Ammo>()) ||
                                            (mode == Mode.Armor && i.GetComponent<Armor>()) ||
                                            (mode == Mode.Accessory && i.GetComponent<Accessory>())
                                    );

        // Clear item buttons
        for (int i = 0; i < itemRow.transform.childCount; ++i)
        {
            itemRow.transform.GetChild(i).GetComponent<Button>().onClick.RemoveAllListeners();
            if (itemRow.transform.GetChild(i).GetComponent<RightClick>() != null)
                Destroy(itemRow.transform.GetChild(i).GetComponent<RightClick>());
        }

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
                GameObject copyRow = Instantiate
                    (
                        itemRow, 
                        i == 0 ? 
                            new Vector2(itemRow.transform.position.x, itemRow.transform.position.y - 30) :
                            new Vector2(copyRows[i - 1].transform.position.x, copyRows[i - 1].transform.position.y - 30), 
                        itemRow.transform.rotation,
                        itemRow.transform.parent
                    );
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
    }
}