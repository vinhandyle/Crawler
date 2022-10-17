using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreatHeal : Spell
{
    public GreatHeal() : base()
    {
        itemTypeID = 3002;
        value = -1;
        sprite = GetSprite("GreatHeal");

        names.Add(0, "Great Heal");
    }
}
