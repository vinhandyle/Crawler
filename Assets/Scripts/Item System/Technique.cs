﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Stats;

/// <summary>
/// Base class for all techniques.
/// </summary>
public abstract class Technique : Item
{
    public AttackType type { get => _type; set => _type = value; }
    [SerializeField] protected AttackType _type;

    public override void SetBaseInfo()
    {
        base.SetBaseInfo();
        stashable = false;
        sellable = false;
        stealable = false;

        SetRequirements(0, 0, 0);
        SetUseCosts(0, 0, 0);
    }

    /// <summary>
    /// Check if this technique can be linked to the specified weapon.
    /// </summary>
    public virtual bool CheckValidLink(Weapon weapon)
    {
        return _type == weapon.type;
    }
}
