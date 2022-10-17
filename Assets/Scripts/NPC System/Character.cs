using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base class for all characters (player, NPCs, interactible objects)
/// </summary>
public abstract class Character : MonoBehaviour
{
    protected Rigidbody2D rb;
    public Inventory inventory { get; protected set; }
    public Equipment equipment { get; protected set; }
    public CharacterProfile profile { get; protected set; }

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        inventory = GetComponent<Inventory>();
        equipment = GetComponent<Equipment>();
        profile = GetComponent<CharacterProfile>();

        SetBaseInventory();
        if (equipment)
        {
            equipment.SetDefaults(this);
            SetBaseEquipment();
        }
    }

    /// <summary>
    /// Set the starting inventory of the character.
    /// </summary>
    protected virtual void SetBaseInventory() { }

    /// <summary>
    /// Set the starting equipment of the character.
    /// </summary>
    protected virtual void SetBaseEquipment() { }
}
