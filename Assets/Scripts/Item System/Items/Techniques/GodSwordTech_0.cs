using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GodSwordTech_0 : Technique
{
    public GodSwordTech_0() : base()
    {
        itemTypeID = -1000;
        value = -1;

        names.Add(0, "???");
        names.Add(100, "Tranquil Storm");

        type = Stats.AttackType.Slash;
    }
}
