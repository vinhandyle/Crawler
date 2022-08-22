using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base class for all (passive) effects.
/// </summary>
public abstract class Effect : MonoBehaviour
{
    public string description { get; protected set; }

    public abstract void SetBaseInfo();
}
