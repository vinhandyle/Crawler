using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankRing : Accessory
{
    public TankRing() : base()
    {
        itemTypeID = 10000; //
        value = -1;
        sprite = GetSprite("TankRing");

        names.Add(0, "Defender's Ring");

        SetRequirements(0, 0, 0);
    }
}
