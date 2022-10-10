using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class KeyItem : Item
{
    public KeyItem()
    {
        sellable = false;
    }

    public static string GetStaticItemClass()
    {
        return "KeyItem";
    }

    public override string GetItemClass()
    {
        return GetStaticItemClass();
    }
}
