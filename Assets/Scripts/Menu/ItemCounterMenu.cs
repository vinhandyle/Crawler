using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Base class for any menus involving stackable items.
/// </summary>
public abstract class ItemCounterMenu : Menu
{
    [Header("Menu")]
    [SerializeField] protected Button incBtn;
    [SerializeField] protected Button decBtn;
    [SerializeField] protected Button confirmBtn;
    [SerializeField] protected Button cancelBtn;
    [SerializeField] protected Text displayInfo;
    [SerializeField] protected Text displayAmt;

    [SerializeField] protected Item item;
    [SerializeField] protected int maxAmt;
    [SerializeField] protected int amt;

    [SerializeField] protected List<Menu> menusToReload;

    protected event Action OnConfirm;

    public override void Load()
    {
        SetDisplayAmount();
        ReloadButtons();
    }

    /// <summary>
    /// Reload the functionality of all buttons in this menu.
    /// </summary>
    protected void ReloadButtons()
    {
        incBtn.onClick.RemoveAllListeners();
        decBtn.onClick.RemoveAllListeners();
        confirmBtn.onClick.RemoveAllListeners();
        cancelBtn.onClick.RemoveAllListeners();

        incBtn.onClick.AddListener(() => { UpdateCounter(1); });
        decBtn.onClick.AddListener(() => { UpdateCounter(-1); });
        confirmBtn.onClick.AddListener(
            () =>
            {
                OnConfirm.Invoke();
                Close();

                foreach (Menu menu in menusToReload) if (menu.open) menu.Load();
            }
        );
        cancelBtn.onClick.AddListener(Close);
    }

    /// <summary>
    /// Set the displayed context info.
    /// </summary>
    protected abstract void SetDisplayInfo();

    #region Counter

    /// <summary>
    /// Update the item counter by the delta amount with overflow control.
    /// </summary>
    private void UpdateCounter(int delta)
    {
        amt += delta;

        if (amt > maxAmt) amt = 1;
        if (amt <= 0) amt = maxAmt;

        SetDisplayAmount();
        SetDisplayInfo();
    }

    /// <summary>
    /// Sets the UI elements related to the item counter.
    /// </summary>
    protected void SetActiveCounterSection(bool b)
    {
        incBtn.gameObject.SetActive(b);
        decBtn.gameObject.SetActive(b);
        displayAmt.gameObject.SetActive(b);
    }

    /// <summary>
    /// Set the displayed counter value.
    /// </summary>
    protected void SetDisplayAmount()
    {
        displayAmt.text = string.Format("{0}/{1}", amt, maxAmt > 0 ? maxAmt.ToString() : "Unlimited");
    }

    #endregion
}
