using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : Ammo
{
    public override void SetBaseInfo()
    {
        base.SetBaseInfo();

        itemTypeID = 2000;
        value = 10;

        names.Add(0, "Arrow");

        baseStats[Stats.Damage.Pierce] = 5;
    }
}
