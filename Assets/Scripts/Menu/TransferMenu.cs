using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Defines the right-click interaction with stackable items during transfers.
/// </summary>
public class TransferMenu : Menu
{ 
    [Header("Visible for Debug")]
    [SerializeField] private InventoryMenu im;
    [SerializeField] private Inventory srcInvenory;
    [SerializeField] private Inventory destInventory;
    [SerializeField] private Item item;
    [SerializeField] private string transferType;

    [Header("Menu")]
    [SerializeField] private Button incBtn;
    [SerializeField] private Button decBtn;
    [SerializeField] private Button confirmBtn;
    [SerializeField] private Button cancelBtn;
    [SerializeField] private Text displayInfo;
    [SerializeField] private Text displayAmt;
    [SerializeField] private int amt;

    [Header("Menus to Reload")]
    [SerializeField] private List<Menu> menusToReload;

    private void Update()
    {
        // Close if inventory menu(s) are closed
        if (im != null && !im.open) Close();
    }

    public override void Open()
    {
        transform.position = im.transform.position;
        base.Open();
    }

    public override void Close()
    {
        im = null;
        base.Close();
    }

    public override void Load()
    {
        SetDisplayAmount();
        ReloadButtons();
    }

    /// <summary>
    /// 
    /// </summary>
    public void Trigger(InventoryMenu menu, Inventory src, Inventory dest, Item item, string type)
    {
        Close();
        if (open && this.item == item) return;  // Don't reload action menu if re-click same target

        im = menu;
        srcInvenory = src;
        destInventory = dest;
        this.item = item;
        transferType = type;

        amt = 1;
        SetDisplayInfo();
        SetActiveTransferAmount(item.stackable);
        Open();
    }

    #region Button Setup

    /// <summary>
    /// Reload the functionality of all buttons in this menu.
    /// </summary>
    private void ReloadButtons()
    {      
        incBtn.onClick.RemoveAllListeners();
        decBtn.onClick.RemoveAllListeners();
        confirmBtn.onClick.RemoveAllListeners();
        cancelBtn.onClick.RemoveAllListeners();

        incBtn.onClick.AddListener(() => { UpdateTransferAmount(1); });
        decBtn.onClick.AddListener(() => { UpdateTransferAmount(-1); });
        confirmBtn.onClick.AddListener(
            () =>
            {
                srcInvenory.TransferItem(destInventory, item, transferType, amt > 0 ? amt : 1);
                Close();

                foreach (Menu menu in menusToReload) menu.Load();
            }
        );
        cancelBtn.onClick.AddListener(Close);
    }

    /// <summary>
    /// Update the current transfer quanity by the delta amount with overflow control.
    /// </summary>
    private void UpdateTransferAmount(int delta)
    {
        amt += delta;

        if (amt > item.quantity) amt = 1;
        if (amt == 0) amt = item.quantity;

        SetDisplayAmount();
        SetDisplayInfo();
    }

    #endregion

    #region Display Setup

    /// <summary>
    /// Set the context info for display.
    /// </summary>
    private void SetDisplayInfo()
    {
        switch (transferType)
        {
            case "Buy":
                displayInfo.text = string.Format("Buy for:\n{0} G", item.value * amt);
                break;

            case "Sell":
                displayInfo.text = string.Format("Sell for:\n{0} G", (int)(item.value * 0.8f) * amt);
                break;

            case "Steal":
                displayInfo.text = string.Format("Chance of success:\n{0}%", Math.Round(1 / ((float)(item.value + 2) * amt), 2));
                break;

            case "Learn":
                displayInfo.text = string.Format("Learn for:\n{0} AP", item.value * amt);
                break;

            default:
                displayInfo.text = (im.transform.parent.name == "Character Menu") ? "Deposit" : "Withdraw";
                break;
        }
    }

    /// <summary>
    /// Set the transfer amount for display.
    /// </summary>
    private void SetDisplayAmount()
    {
        displayAmt.text = string.Format("{0}/{1}", amt, item.quantity);
    }

    /// <summary>
    /// Sets the UI elements related to transferring higher quantities active or not.
    /// </summary>
    private void SetActiveTransferAmount(bool b)
    {
        incBtn.gameObject.SetActive(b);
        decBtn.gameObject.SetActive(b);
        displayAmt.gameObject.SetActive(b);
    }

    #endregion
}
