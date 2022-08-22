using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static ActionMenu;

/// <summary>
/// Base class for all non-playable characters (including chests and corpses).
/// </summary>
public class NPC : MonoBehaviour
{
    [Header("Action")]
    [SerializeField] private List<Action> actions;
    [SerializeField] private ActionMenu am;
    [SerializeField] protected bool hideFromScan;
    [SerializeField] protected bool isVendor;


    [Header("Player")]
    [SerializeField] protected Collider2D playerSight;
    [SerializeField] protected Collider2D playerReach;
    [SerializeField] protected bool inSight;
    [SerializeField] protected bool inReach;
    [SerializeField] protected bool noAntiHide;
     
    private void Awake()
    {
        actions = new List<Action>();
        if (GetComponent<Equipment>() != null) actions.Add(Action.Scan);
        if (GetComponent<Dialogue>() != null) actions.Add(Action.Talk);
        if (GetComponent<Inventory>() != null)
        {
            if (GetComponent<Equipment>() == null)
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
        playerSight = FindObjectOfType<PlayerController>().transform.Find("Sight").GetComponent<Collider2D>();
        playerReach = FindObjectOfType<PlayerController>().transform.Find("Reach").GetComponent<Collider2D>();
        noAntiHide = false; // Change to check player scan skill
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
        bool inReachAction = inReach && (actions.Any(a => new List<Action>() { Action.Talk, Action.Loot, Action.Trade, Action.Steal }.Contains(a)));
        bool inSightAction = inSight && actions.Contains(Action.Scan);

        CustomCursor.Instance.SetCursor((inReachAction || inSightAction) ? 2 : 1);

        if (inReach)
        {
            if (Input.GetMouseButtonDown(1))
            {
                // Add check for super scan skill
                if (hideFromScan && noAntiHide) tempActions.Remove(Action.Scan);

                am.Trigger(tempActions, this);
            }
        }
        else if (inSight)
        {
            tempActions.RemoveAll(a => new List<Action>() { Action.Talk, Action.Loot, Action.Trade, Action.Steal }.Contains(a));

            if (Input.GetMouseButtonDown(1))
            {
                // Don't do anything if opening nonexistent/hidden equipment
                if (GetComponent<Equipment>() != null)
                {
                    am.Trigger(tempActions, this);
                }
            }
        }
    }

    private void OnMouseExit()
    {
        CustomCursor.Instance.SetCursor(0);
    }
}
