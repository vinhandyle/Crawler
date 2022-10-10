using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldCoin : KeyItem
{
    public GoldCoin(int quantity) : base()
    {
        itemTypeID = -1;
        value = 1;

        stackable = true;
        buyable = false;
        this.quantity = quantity;

        names.Add(0, "Gold Coin");

        descriptions.Add(0, "The currency of these lands.");
    }
}
