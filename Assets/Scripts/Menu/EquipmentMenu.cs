using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Defines the equipment UI.
/// </summary>
public class EquipmentMenu : CharacterMenu
{
    [SerializeField] private Equipment equipment;

    /// <summary>
    /// Set the target data to the specified equipment.
    /// </summary>
    public void Set(Equipment equipment)
    {
        this.equipment = equipment;
    }

    public override void Load()
    {
        base.Load();

        // Load equipment
    }
}
