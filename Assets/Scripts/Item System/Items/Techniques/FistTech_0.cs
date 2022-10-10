using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FistTech_0 : Technique
{
    public FistTech_0() : base()
    {
        itemTypeID = -1001;
        value = -1;

        names.Add(0, "Praying Strikes");

        type = Stats.AttackType.Strike;
    }
}
