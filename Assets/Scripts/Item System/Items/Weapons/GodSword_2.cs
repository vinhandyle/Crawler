using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GodSword_2 : Weapon
{
    public GodSword_2() : base()
    {
        itemTypeID = 1002; //
        value = -1;
        sprite = GetSprite("GodSword_2");
        _type = Stats.AttackType.Slash;

        //tech = new GodSwordTech_2();

        names.Add(0, "Black Blade");
        names.Add(40, "Otherworldly Blade");
        names.Add(60, "Fragment of the Night Sky");
        names.Add(100, "Nachtflugel, the Final Blade");

        SetRequirements(0, 30, 50); //

        baseStats[Stats.Damage.Physical] = 20; //
        scalings[Stats.Damage.Physical] = new float[3] { 0.1f, 2, 0 }; //

        baseSpellDmg = 100; //
        spellScaling = 0.1f; //
    }
}
