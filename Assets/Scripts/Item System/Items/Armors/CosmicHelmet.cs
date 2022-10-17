using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CosmicHelmet : Armor
{
    public CosmicHelmet() : base()
    {
        itemTypeID = 60000000; //
        value = -1;
        sprite = GetSprite("CosmicHelmet");

        names.Add(0, "Strange Creature");
        names.Add(30, "Brain Parasite");
        names.Add(60, "Mind-Expanding Parasite");

        SetRequirements(0, 0, 0);
    }
}
