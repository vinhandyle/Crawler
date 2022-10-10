using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Defines the item equip menu.
/// </summary>
public class EquipMenu : Menu
{
    [SerializeField] private Inventory inventory;
    [SerializeField] private List<InfoMenu> infoMenus;
    [SerializeField] private EquipmentMenu em;

    [Header("Menu")]
    [SerializeField] private string itemType;
    [SerializeField] private Button equipBtn;
    [SerializeField] private Button unequipBtn;
    [SerializeField] private Button cancelBtn;
    [SerializeField] private GameObject itemRow;
    [SerializeField] private List<GameObject> copyRows;

    [Header("Equip")]
    [SerializeField] private Item currentEquipped;
    [SerializeField] private Item newEquip;
    public event Action<Item, string> OnConfirm;

    [Header("Link")]
    [SerializeField] private bool linkMode;
    [SerializeField] private Button modeBtn;
    [SerializeField] private Item itemToLink;
    [SerializeField] private Technique currentLinked;
    [SerializeField] private Technique newLink;

    protected override void Awake()
    {
        modeBtn.onClick.AddListener(SwitchMode);
        base.Awake();
    }

    public override void Close()
    {
        OnConfirm = null;
        foreach (InfoMenu menu in infoMenus) menu.Close();
        base.Close();
    }

    /// <summary>
    /// Performs any pre-processing for he item equip menu before opening it.
    /// </summary>
    public void Trigger(Item item, string type)
    {
        itemType = type;
        currentEquipped = item;
        newEquip = null;

        linkMode = false;
        currentLinked = ((Weapon)item)?.tech;
        newLink = null;

        transform.position = em.transform.position;
        Open();
    }

    #region Button Setup

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
                    // Selecting same item will close comparison info menu
                    if (infoMenus[0].CompareItem(item))
                    {
                        infoMenus[0].Close();
                        if (item.GetItemClass() == Weapon.GetStaticItemClass())
                        {
                            itemToLink = currentEquipped;
                            currentLinked = ((Weapon)itemToLink)?.tech;
                        }
                    }
                    else
                    {
                        infoMenus[0].SetItem(item, inventory.GetComponent<Equipment>(), 1);
                        if (item.GetItemClass() == Weapon.GetStaticItemClass())
                        {
                            itemToLink = item;
                            currentLinked = ((Weapon)itemToLink).tech;
                        }
                    }
                    modeBtn.gameObject.SetActive(!linkMode && itemToLink != null);

                    // Set tentative equip/link
                    if (linkMode)
                    {
                        newLink = (Technique)item;
                    }
                    else
                    {
                        newEquip = item;
                    }
                    equipBtn.gameObject.SetActive(true);
                }
            );
    }

    /// <summary>
    /// Switch between the equip and link modes.
    /// </summary>
    private void SwitchMode()
    {
        linkMode = !linkMode;
        if (linkMode)
        {
            newLink = null;
            infoMenus[0].Close();
            infoMenus[1].SetItem(((Weapon)itemToLink).tech, inventory.GetComponent<Equipment>());
        }

        Load();
    }

    /// <summary>
    /// Reload the functionality of all buttons in this menu.
    /// </summary>
    protected void ReloadButtons()
    {
        equipBtn.gameObject.SetActive((!linkMode && newEquip != null) || (linkMode && newLink != null));
        unequipBtn.gameObject.SetActive((!linkMode && currentEquipped != null) || (linkMode && currentLinked != null));
        modeBtn.gameObject.SetActive(!linkMode && (Weapon)itemToLink != null);

        equipBtn.onClick.RemoveAllListeners();
        unequipBtn.onClick.RemoveAllListeners();
        cancelBtn.onClick.RemoveAllListeners();

        equipBtn.GetComponentInChildren<Text>().text = linkMode ? "Link" : "Equip";
        equipBtn.onClick.AddListener
            (
                () =>
                {
                    if (linkMode)
                    {
                        ((Weapon)itemToLink).tech = newLink;
                        currentLinked = newLink;
                        infoMenus[0].SetItem(itemToLink, inventory.GetComponent<Equipment>(), 1);
                        infoMenus[1].SetItem(currentEquipped, inventory.GetComponent<Equipment>());
                        SwitchMode();
                    }
                    else
                    {
                        inventory.AddItem(currentEquipped);
                        OnConfirm.Invoke(newEquip, itemType);
                        inventory.RemoveItem(newEquip);

                        currentEquipped = newEquip;
                        infoMenus[0].Close();
                        infoMenus[1].SetItem(currentEquipped, inventory.GetComponent<Equipment>());
                        em.Load();
                        Load();
                    }
                }
            );

        unequipBtn.GetComponentInChildren<Text>().text = linkMode ? "Delink" : "Unequip";
        unequipBtn.onClick.AddListener
            (
                () =>
                {
                    if (linkMode)
                    {
                        ((Weapon)itemToLink).tech = null;
                        currentLinked = null;
                        infoMenus[0].SetItem(itemToLink, inventory.GetComponent<Equipment>(), 1);
                        if (currentEquipped != null)
                            infoMenus[1].SetItem(currentEquipped, inventory.GetComponent<Equipment>());
                        else
                            infoMenus[1].Close();
                        SwitchMode();
                    }
                    else
                    {
                        inventory.AddItem(currentEquipped);
                        OnConfirm.Invoke(null, itemType);

                        currentEquipped = null;
                        infoMenus[1].Close();
                        em.Load();
                        Load();
                    }
                }
            );

        cancelBtn.onClick.AddListener
            (
                () =>
                {
                    foreach (InfoMenu menu in infoMenus)
                        menu.Close();

                    if (linkMode)
                    {
                        infoMenus[0].SetItem(itemToLink, inventory.GetComponent<Equipment>(), 1);
                        if (currentEquipped != null)
                            infoMenus[1].SetItem(currentEquipped, inventory.GetComponent<Equipment>());
                        else
                            infoMenus[1].Close();
                        SwitchMode();
                    }
                    else
                    {
                        Close();
                    }
                }
            );
    }

    #endregion

    public override void Load()
    {
        ReloadButtons();

        #region Item Buttons

        // Get items to display
        List<Item> items = inventory.GetAllItems()
                                    .FindAll
                                    (
                                        i =>
                                            (!linkMode && itemType == i.GetItemClass()) ||
                                            (linkMode && i.GetItemClass() == Technique.GetStaticItemClass() && ((Technique)i).CheckValidLink((Weapon)itemToLink)) ||
                                            (new List<string>() { "Helmet", "Chestplate", "Leggings", "Boots" }.Contains(itemType) && i.GetItemType()[0] == itemType)
                                    )
                                    .OrderBy(item => item.itemTypeID)
                                    .ToList();

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
