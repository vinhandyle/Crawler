using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class KeyItem : Item
{
    public override void SetBaseInfo()
    {
        base.SetBaseInfo();
        sellable = false;
    }

    public override Dictionary<Stats.Damage, int> GetStats(int str, int dex, int @int)
    {
        throw new System.NotImplementedException();
    }
}
