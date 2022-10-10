using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : Ammo
{
    public Arrow(int quantity) : base(quantity)
    {
        itemTypeID = 2000;
        value = 10;

        names.Add(0, "Arrow");

        baseStats[Stats.Damage.Physical] = 5;
    }
}
