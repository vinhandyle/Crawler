using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Defines the right-click interaction with transferable items.
/// </summary>
public class TransferMenu : ItemCounterMenu
{ 
    [Header("Visible for Debug")]
    [SerializeField] private InventoryMenu im;
    [SerializeField] private Inventory srcInvenory;
    [SerializeField] private Inventory destInventory;
    [SerializeField] private string transferType;

    protected override void Awake()
    {
        OnConfirm += () => { srcInvenory.TransferItem(destInventory, item, transferType, amt > 0 ? amt : 1); };
        base.Awake();
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

    /// <summary>
    /// Performs any pre-processing for the transfer menu before opening it.
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

        // QoL for trade-dumping
        if (type == "Buy" || type == "Sell")
            maxAmt = (int)(destInventory.coins / TransferManager.Instance.GetTransferInfo(item, type, 1));
        else
            maxAmt = item.quantity;
        maxAmt = (maxAmt <= 0) ? 1 : (maxAmt > item.quantity) ? item.quantity : maxAmt;

        amt = 1;
        SetDisplayInfo();
        SetActiveCounterSection(item.stackable);
        Open();
    }

    protected override void SetDisplayInfo()
    {
        var tm = TransferManager.Instance;

        switch (transferType)
        {
            case "Buy":
                displayInfo.text = string.Format("Buy for:\n{0} G", tm.GetTransferInfo(item, transferType, amt));
                break;

            case "Sell":
                displayInfo.text = string.Format("Sell for:\n{0} G", tm.GetTransferInfo(item, transferType, amt));
                break;

            case "Steal":
                displayInfo.text = string.Format("Chance of success:\n{0}%", tm.GetTransferInfo(item, transferType, amt));
                break;

            case "Learn":
                displayInfo.text = string.Format("Learn for:\n{0} AP", tm.GetTransferInfo(item, transferType, amt));
                break;

            default:
                displayInfo.text = (im.transform.parent.name == "Character Menu") ? "Deposit" : "Withdraw";
                break;
        }
    }
}
