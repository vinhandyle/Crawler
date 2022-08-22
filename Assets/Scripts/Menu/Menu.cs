using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base class for all in-game menus.
/// </summary>
public abstract class Menu : MonoBehaviour
{
    public bool open { get; protected set; }

    protected virtual void Awake()
    {
        Close();
    }

    /// <summary>
    /// Use supplied information to load the content in the menu.
    /// </summary>
    public abstract void Load();

    /// <summary>
    /// Open the menu.
    /// </summary>
    public virtual void Open()
    {
        open = true;
        Load();
        gameObject.SetActive(true);
    }

    /// <summary>
    /// Close the menu.
    /// </summary>
    public virtual void Close()
    {
        open = false;
        gameObject.SetActive(false);
    }
}
