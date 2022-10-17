using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CosmicChestplate : Armor
{
    public CosmicChestplate() : base()
    {
        itemTypeID = 60100000; //
        value = -1;
        sprite = GetSprite("CosmicChestplate");

        names.Add(0, "Strange Creature");
        names.Add(30, "Spine Parasite");
        names.Add(60, "Body-Contorting Parasite");

        SetRequirements(0, 0, 0);
    }
}
