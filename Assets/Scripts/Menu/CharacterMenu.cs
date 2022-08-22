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
        if (connMenus.Count == 2 && connMenus[1] != null && !connMenus[1].open) connMenus[1].Open(); 
    }

    public override void Close()
    {
        //Debug.Log(transform.parent + " -> " + gameObject.name);
        base.Close();
        infoMenu.Close();
        if (connMenus.Count == 2 && connMenus[0] != null && connMenus[0].open) connMenus[0].Close();
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
        altMenu.Open();
        Close();
    }
}
