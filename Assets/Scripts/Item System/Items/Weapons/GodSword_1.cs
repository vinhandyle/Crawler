using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GodSword_1 : Weapon
{
    public GodSword_1() : base()
    {
        itemTypeID = 1001; //
        value = -1;
        sprite = GetSprite("GodSword_1");
        _type = Stats.AttackType.Slash;

        //tech = new GodSwordTech_1();

        names.Add(0, "Bloodstained Blade");
        names.Add(40, "Demonic Blade");
        names.Add(100, "Blutmond, the Flesh Blade");

        SetRequirements(20, 20, 20); //

        baseStats[Stats.Damage.Physical] = 20; //
        scalings[Stats.Damage.Physical] = new float[3] { 0.1f, 2, 0 }; //
    }
}
