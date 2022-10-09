using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base class for all spells.
/// </summary>
public abstract class Spell : Item
{
    public override void SetBaseInfo()
    {
        base.SetBaseInfo();
        stashable = false;
        sellable = false;
        stealable = false;

        SetRequirements(0, 0, 0);
        SetUseCosts(0, 0, 0);
    }
}
