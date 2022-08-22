using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Defines the right-click interaction with NPCs (which may involve a menu).
/// </summary>
public class ActionMenu : Menu
{
    [Header("Visible for Debug")]
    [SerializeField] private EquipmentMenu playerEm;
    [SerializeField] private InventoryMenu playerIm;
    [SerializeField] private EquipmentMenu extEm;
    [SerializeField] private InventoryMenu extIm;
    [SerializeField] private NPC target;
    [SerializeField] private List<Action> actions;

    [Header("Components")]
    [SerializeField] private Button closeBtn;
    [SerializeField] private List<Button> btns;

    public enum Action
    {
        Scan,
        Talk,
        Loot,
        Trade,
        Steal
    }

    protected override void Awake()
    {
        playerEm = GameObject.Find("Character Menu").GetComponentInChildren<EquipmentMenu>();
        playerIm = GameObject.Find("Character Menu").GetComponentInChildren<InventoryMenu>();
        extEm = GameObject.Find("Ext Menu").GetComponentInChildren<EquipmentMenu>();
        extIm = GameObject.Find("Ext Menu").GetComponentInChildren<InventoryMenu>();

        closeBtn.onClick.AddListener( Close );
        base.Awake();
    }

    public override void Open()
    {
        open = true;
        gameObject.SetActive(true);
    }

    public override void Load()
    {
        foreach (Button btn in btns)
        {
            btn.onClick.RemoveAllListeners();
            btn.gameObject.SetActive(false);
        }
        Open();

        for (int i = 0; i < actions.Count; ++i)
        {
            Button btn = btns[i]; // Need to access a temp to prevent listener arg overwrite
            btns[i].gameObject.SetActive(true);
            btns[i].GetComponentInChildren<Text>().text = actions[i].ToString();
            btns[i].onClick.AddListener(() => { SelectAction(btn); });
        }
    }

    /// <summary>
    /// Start the process of an action.
    /// If there are multiple possible actions, open a menu to select.
    /// Otherwise, perform the action immediately.
    /// </summary>
    public void Trigger(List<Action> actions, NPC target)
    {
        Close();
        if (open && this.target == target) return;  // Don't reload action menu if re-click same target

        // Set target of action
        if (target != this.target)
        {
            extEm.Close();
        }
        SetTarget(target);

        // Perform action or open action menu
        if (actions.Count > 1)
        {
            this.actions = actions;
            Load();
        }
        else
        {
            switch (actions[0])
            {
                case Action.Scan:
                    ToggleScan(false);
                    break;

                case Action.Talk:
                    ToggleTalk();
                    break;

                case Action.Loot:
                case Action.Trade:
                case Action.Steal:
                    ToggleLoot(actions[0].ToString());
                    break;
            }
        }
    }

    #region Action Setup

    /// <summary>
    /// 
    /// </summary>
    public NPC CurrentTarget()
    {
        return target;
    }

    /// <summary>
    /// Sets the target of the external menu to this NPC.
    /// </summary>
    public void SetTarget(NPC target)
    {
        this.target = target;
        extIm.Set(target.GetComponent<Inventory>());
        extEm.Set(target.GetComponent<Equipment>());
    }

    /// <summary>
    /// Attach to a button to select an action from a list.
    /// </summary>
    private void SelectAction(Button btn)
    {
        extEm.Close();
        extIm.Close();

        switch (btn.GetComponentInChildren<Text>().text)
        {
            case "Scan":
                ToggleScan(true);
                break;

            case "Talk":
                ToggleTalk();
                break;

            case "Loot":
            case "Trade":
            case "Steal":
                ToggleLoot(btn.GetComponentInChildren<Text>().text);
                break;
        }

        Close();
    }

    /// <summary>
    /// Close the ext menu associated with the given action
    /// </summary>
    public void CloseExtMenu(Action action)
    {
        switch (action)
        {
            case (Action.Scan):
                extEm.Close();
                break;

            case (Action.Loot):
                if (extIm.open) extIm.Close();
                extEm?.SetSwitchable(false);

                // Reload switch view button
                bool open = extEm.open;
                extEm.Open();
                if (!open) extEm.Close();
                break;
        }
        Close();
    }

    /// <summary>
    /// 
    /// </summary>
    public void ReloadSwitchable()
    {
        if (extEm.open)
        {
            extEm.SetSwitchable(!target.GetComponent<Inventory>().standalone);
            extEm.Open();
        }
    }

    #endregion

    #region Action

    /// <summary>
    /// Toggle the ext equipment menu.
    /// </summary>
    private void ToggleScan(bool canSwitch)
    {
        extEm.SetSwitchable(canSwitch);

        // Cannot view equipment of chest/corpses
        if (!target.GetComponent<Inventory>().standalone)
        {
            extEm.Toggle();
        }
    }

    /// <summary>
    /// Toggle the ext dialogue menu.
    /// </summary>
    private void ToggleTalk()
    {

    }

    /// <summary>
    /// Toggle the ext inventory menu.
    /// </summary>
    private void ToggleLoot(string type)
    {
        extEm.SetSwitchable(true);

        if (extEm.open)
        {
            extEm.Toggle();
        }
        else
        {
            extIm.Toggle();

            // Always switch player to inventory
            if (playerEm.open)
                playerEm.Close();

            // Player inventory opens/closes with external
            if (extIm.open)
                playerIm.Open();
            else
                playerIm.Close();

            // Set transfer type
            extIm.transferType = (type == "Trade") ? "Buy" : type;
            playerIm.transferType = (type == "Trade") ? "Sell" : type;
        }
    }

    #endregion
}
