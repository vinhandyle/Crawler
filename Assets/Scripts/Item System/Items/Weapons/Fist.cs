using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fist : Weapon
{
    public Fist() : base()
    {
        itemTypeID = 0;
        value = 0;
        sprite = GetSprite("Fist");

        lootable = false;
        stashable = false;
        buyable = false;
        sellable = false;
        stealable = false;

        names.Add(0, "Fist");

        baseStats[Stats.Damage.Physical] = 1;
        scalings[Stats.Damage.Physical] = new float[3] { 0.5f, 0, 0 };
    }
}
