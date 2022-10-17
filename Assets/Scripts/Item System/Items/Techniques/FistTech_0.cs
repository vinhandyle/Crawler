using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FistTech_0 : Technique
{
    public FistTech_0() : base()
    {
        itemTypeID = -1001;
        value = -1;
        sprite = GetSprite("FistTech_0");

        names.Add(0, "Praying Strikes");

        string baseDescription = "2-hit combo with moderate knockback.";
        descriptions.Add(
            0, 
            string.Format(
                "{0}\n\n{1}",
                baseDescription,
                "The most basic of the techniques taught to the monks of Senpou Temple."
                ) 
            );

        type = Stats.AttackType.Strike;
    }
}
