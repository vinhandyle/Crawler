using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlashTech_0 : Technique
{
    public SlashTech_0() : base()
    {
        itemTypeID = -1002;
        value = -1;
        sprite = GetSprite("SlashTech_0");

        names.Add(0, "Spinning Slash");

        type = Stats.AttackType.Slash;
    }
}
