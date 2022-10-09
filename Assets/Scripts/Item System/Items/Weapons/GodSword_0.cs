using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GodSword_0 : Weapon
{
    public override void SetBaseInfo()
    {
        base.SetBaseInfo();

        itemTypeID = 1000; //
        value = -1;
        _type = Stats.AttackType.Slash;

        names.Add(0, "Sharp Blade");
        names.Add(20, "Magnificent Blade");
        names.Add(40, "Divine Blade");
        names.Add(100, "Hochste, the First Blade");

        SetRequirements(10, 30, 0);

        baseStats[Stats.Damage.Physical] = 10;
        scalings[Stats.Damage.Physical] = new float[3] { 0.1f, 2, 0 }; //
    }
}
