using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potion : Consumable
{
    public Potion(int quantity) : base()
    {
        itemTypeID = 5000;
        value = 20;
        this.quantity = quantity;

        stackable = true;

        names.Add(0, "Potion");
    }

    public override void Use(int amt)
    {
        Debug.Log("Used potion.");
    }
}
