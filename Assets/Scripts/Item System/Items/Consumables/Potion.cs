using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potion : Consumable
{
    public override void SetBaseInfo()
    {
        base.SetBaseInfo();

        itemTypeID = 5000;
        value = 20;

        stackable = true;

        names.Add(0, "Potion");
    }

    public override void Use(int amt)
    {
        Debug.Log("Used potion.");
    }
}
