using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GodSwordTech_0 : Technique
{
    public GodSwordTech_0() : base()
    {
        itemTypeID = -1000;
        value = -1;
        sprite = GetSprite("GodSwordTech_0");

        names.Add(0, "???");
        names.Add(100, "Tranquil Storm");

        descriptions.Add(0, "Sealed from the unworthy.");
        descriptions.Add(
            100, 
            string.Format(
                "{0}\n\n{1}",
                "Concentrate for a brief moment then unleash a flurry of attacks.",
                "After decades of meditation atop the world's tallest peak, the Technique God had an epiphany."
                )
            );

        type = Stats.AttackType.Slash;
    }

    public override bool CheckValidLink(Weapon weapon)
    {
        return weapon.GetType() == typeof(GodSword_0);
    }
}
