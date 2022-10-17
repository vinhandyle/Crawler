using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Base class for the character menus.
/// </summary>
public abstract class CharacterMenu : Menu
{
    [SerializeField] protected InfoMenu infoMenu;

    [Header("Configurable")]
    [SerializeField] protected CharacterMenu altMenu;
    [SerializeField] protected List<CharacterMenu> connMenus;

    [SerializeField] protected Button btn;
    [SerializeField] protected bool switchable = true;

    [Header("Switch Event")]
    [SerializeField] protected bool allowSwitchEvent = true;
    protected event Action OnSwitch;

    protected override void Awake()
    {
        infoMenu = FindObjectOfType<Canvas>().transform.Find("Info Menu").GetComponent<InfoMenu>();
        btn.onClick.AddListener(SwitchView);
        base.Awake();
    }

    public override void Load()
    {
        btn.gameObject.SetActive(switchable);
    }

    public override void Open()
    {
        base.Open();
        ToggleConnectedMenu(1, true); 
    }

    public override void Close()
    {
        base.Close();
        infoMenu.Close();
        ToggleConnectedMenu(0, false);
    }

    /// <summary>
    /// Toggle the connected menu at the specified index to be in the specified state.
    /// </summary>
    /// <param name="index"></param>
    /// <param name="open"></param>
    protected void ToggleConnectedMenu(int index, bool open)
    {
        if (connMenus.Count >= index + 1 && connMenus[index] != null)
        {
            if (open)
            {
                if (!connMenus[index].open) connMenus[index].Open();
            }
            else
            {
                if (connMenus[index].open) connMenus[index].Close();
            }
        }
    }

    /// <summary>
    /// Set the switchable status of this menu.
    /// </summary>
    public void SetSwitchable(bool switchable)
    {
        this.switchable = switchable;
    }

    /// <summary>
    /// Toggle the open status of this menu.
    /// </summary>
    public void Toggle()
    {
        if (altMenu.open) altMenu.Close();

        if (open)
            Close();
        else
            Open();
    }

    /// <summary>
    /// Switch between this menu and its alternative.
    /// </summary>
    public void SwitchView()
    {
        // Preserve transfer mode between view switches
        if (allowSwitchEvent)
        {
            altMenu.OnSwitch?.Invoke();
            if (altMenu.connMenus.Count > 0) altMenu.connMenus[0].OnSwitch?.Invoke();
        }
        // Reset transfer mode when starting from outside switch cycle
        else
        {
            altMenu.GetComponent<InventoryMenu>()?.ClearTransferTypes();
            if (altMenu.connMenus.Count > 0)
                altMenu.connMenus[0].GetComponent<InventoryMenu>()?.ClearTransferTypes();
        }
        altMenu.Open();

        if (allowSwitchEvent && altMenu.connMenus.Count > 0 && altMenu.connMenus[0].GetComponent<InventoryMenu>()?.transferType != "")
        {
            altMenu.connMenus[0].Open();
        }
        Close();
    }
}
