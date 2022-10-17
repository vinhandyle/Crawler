using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base class for all (passive) effects.
/// </summary>
public abstract class Effect : MonoBehaviour
{
    protected string effectName;
    protected string description;

    public enum Type
    { 
        Innate,
        Item,
        Buff,
        Debuff
    }
    public Type effectType;

    public abstract void SetBaseInfo();

    /// <summary>
    /// Return effect's type, name, and description formatted as a string.
    /// </summary>
    public string GetDescripton()
    {
        return string.Format(
            "[{0}] {1}: {2}",
            effectType,
            effectName,
            description
            );
    }
}
