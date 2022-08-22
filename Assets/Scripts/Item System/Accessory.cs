using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Stats;

/// <summary>
/// Base class for all accessories.
/// </summary>
public abstract class Accessory : Item
{
    public override Dictionary<Damage, int> GetStats(int str, int dex, int @int)
    {
        throw new System.NotImplementedException();
    }
}
