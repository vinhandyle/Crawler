using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonMaterial : Material
{
    public DragonMaterial(int quantity) : base(quantity)
    {
        itemTypeID = 20000; //
        value = -1;
        sprite = GetSprite("DragonMaterial");

        names.Add(0, "Dragon Scale");
    }
}
