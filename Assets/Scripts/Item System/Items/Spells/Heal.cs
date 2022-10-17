using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heal : Spell
{
    public Heal() : base()
    {
        itemTypeID = 3001;
        value = -1;
        sprite = GetSprite("Heal");

        names.Add(0, "Heal");
    }
}
