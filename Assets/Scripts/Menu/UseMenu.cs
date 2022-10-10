using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Defines the right-click interaction with consumables.
/// </summary>
public class UseMenu : ItemCounterMenu
{
    [Header("Parent")]
    [SerializeField] private InventoryMenu im;
    [SerializeField] private Equipment player;

    protected override void Awake()
    {
        OnConfirm += () => 
        { 
            if (item.quantity >= amt)
            {
                ((Consumable)item).Use(amt);
                item.quantity -= amt;

                if (item.quantity == 0)
                {
                    player.GetComponent<Inventory>().RemoveItem(item);
                }
            }
        };
        base.Awake();
    }

    public override void Open()
    {
        transform.position = im.transform.position;
        base.Open();
    }

    /// <summary>
    /// Performs any pre-processing for the use menu before opening it.
    /// </summary>
    public void Trigger(Item item)
    {
        Close();
        if (open && this.item == item) return;  // Don't reload action menu if re-click same target

        this.item = item;
        maxAmt = item.quantity;
        amt = 1;
        SetDisplayInfo();
        SetActiveCounterSection(item.stackable);
        Open();
    }

    protected override void SetDisplayInfo()
    {
        displayInfo.text = "Use Item";
    }
}
