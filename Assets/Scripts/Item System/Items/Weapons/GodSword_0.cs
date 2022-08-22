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

        names.Add(0, "Sharp Blade");
        names.Add(20, "Magnificent Blade");
        names.Add(40, "Divine Blade");
        names.Add(100, "Hochste, the First Blade");

        reqs[Stats.Stat.Str] = 10; //
        reqs[Stats.Stat.Dex] = 30; //

        baseStats[Stats.Damage.Slash] = 10;
        scalings[Stats.Damage.Slash] = new float[3] { 0.1f, 2, 0 }; //
    }
}
