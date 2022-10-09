using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldCoin : KeyItem
{
    public override void SetBaseInfo()
    {
        base.SetBaseInfo();

        itemTypeID = -1;
        value = 1;

        stackable = true;
        buyable = false;

        names.Add(0, "Gold Coin");

        descriptions.Add(0, "The currency of these lands.");
    }
}
