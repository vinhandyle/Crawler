using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlashTech_0 : Technique
{
    public override void SetBaseInfo()
    {
        base.SetBaseInfo();

        itemTypeID = -1002;
        value = -1;

        names.Add(0, "Spinning Slash");

        type = Stats.AttackType.Slash;
    }
}
