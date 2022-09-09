using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Consumable : Item
{
    public override Dictionary<Stats.Damage, int> GetStats(int str, int dex, int @int)
    {
        throw new System.NotImplementedException();
    }

    /// <summary>
    /// Use the specified amount of this item.
    /// </summary>
    public abstract void Use(int amt);
}
