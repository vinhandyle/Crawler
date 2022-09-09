using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fist : Weapon
{
    public override void SetBaseInfo()
    {
        base.SetBaseInfo();

        itemTypeID = 0;
        value = 0;

        lootable = false;
        buyable = false;
        sellable = false;
        stealable = false;

        names.Add(0, "Fist");

        baseStats[Stats.Damage.Strike] = 1;
        scalings[Stats.Damage.Strike] = new float[3] { 0.5f, 0, 0 };
    }
}
