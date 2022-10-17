using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : Spell
{
    public Fireball() : base()
    {
        itemTypeID = 3000;
        value = -1;
        sprite = GetSprite("Fireball");

        names.Add(0, "Fireball");
    }
}
