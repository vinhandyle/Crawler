using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static ActionMenu;

/// <summary>
/// Base class for all non-playable characters (including chests and corpses).
/// </summary>
public abstract class NPC : Character
{
    [Header("Action")]
    [SerializeField] private List<Action> actions;
    [SerializeField] private ActionMenu am;
    [SerializeField] protected bool isHidden;
    [SerializeField] protected bool isVendor;

    [Header("Dialogue")]
    [SerializeField] protected Dialogue dialogue;

    [Header("Player")]
    [SerializeField] protected CharacterProfile player;
    [SerializeField] protected Collider2D playerSight;
    [SerializeField] protected Collider2D playerReach;
    [SerializeField] protected bool inSight;
    [SerializeField] protected bool inReach;
     
    protected override void Awake()
    {
        base.Awake();

        actions = new List<Action>();
        if (equipment) actions.Add(Action.Scan);
        if (dialogue) actions.Add(Action.Talk);
        if (inventory)
        {
            if (!equipment)
            {
                actions.Add(Action.Loot);
            }
            else
            {
                if (isVendor) actions.Add(Action.Trade);
                actions.Add(Action.Steal);
            }
        }

        am = GameObject.Find("Helper Menus").transform.Find("Action Menu").GetComponentInChildren<ActionMenu>();
        player = FindObjectOfType<PlayerController>().GetComponent<CharacterProfile>();
        playerSight = player.transform.Find("Sight").GetComponent<Collider2D>();
        playerReach = player.transform.Find("Reach").GetComponent<Collider2D>();
    }

    protected override void SetBaseInventory()
    {
        base.SetBaseInventory();

        inventory.AddItem(new GoldCoin(1000));
    }

    private void Update()
    {
        inSight = GetComponent<Collider2D>().IsTouching(playerSight);
        inReach = GetComponent<Collider2D>().IsTouching(playerReach);

        // Exit range for action menus
        if (am.CurrentTarget() == this)
        {
            if (!inReach) am.CloseExtMenu(Action.Loot);
            else am.ReloadSwitchable();

            if (!inSight) am.CloseExtMenu(Action.Scan);
        }
    }

    private void OnMouseOver()
    {
        List<Action> tempActions = new List<Action>(actions);
        bool canScan = profile ? profile.SectionIsVisible(0, player, isHidden) : false;
        if (!canScan) tempActions.Remove(Action.Scan);

        bool inReachAction = inReach && tempActions.Any(a => new List<Action>() { Action.Talk, Action.Loot, Action.Trade, Action.Steal }.Contains(a));
        bool inSightAction = inSight && tempActions.Contains(Action.Scan);

        CustomCursor.Instance.SetCursor((inReachAction || inSightAction) ? 2 : 1);

        if (Input.GetMouseButtonDown(1))
        {
            if (inReach)
            {
                if (isHidden && !canScan)
                {
                    // Trigger mimic
                    Debug.Log("Mimic triggered");
                }
                else
                {
                    am.Trigger(tempActions, this, canScan);
                }
            }
            else if (inSight)
            {
                tempActions.RemoveAll(a => new List<Action>() { Action.Talk, Action.Loot, Action.Trade, Action.Steal }.Contains(a));

                if (canScan && equipment)
                {
                    am.Trigger(tempActions, this, canScan);
                }
            }
        }    
    }

    private void OnMouseExit()
    {
        CustomCursor.Instance.SetCursor(0);
    }
}
