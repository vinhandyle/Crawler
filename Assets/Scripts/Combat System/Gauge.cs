using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Defines functionality related to health, mana, and stamina.
/// </summary>
public class Gauge : MonoBehaviour
{
    [SerializeField] private CharacterProfile character;

    #region Resource Bars

    [SerializeField] private float healthScale = 1;
    public int health;
    public int maxHealth 
    {
        get
        {
            int vit = character.GetStatPoints()[Stats.Stat.Vit];
            return (int)(((vit + 10) * (healthScale + vit * 0.01f) + character.healthBonus) * character.healthMult);
        }
    }

    [SerializeField] private float manaScale = 1;
    public int mana;
    public int maxMana
    {
        get => (int)(((character.GetStatPoints()[Stats.Stat.Arc] + 10) * manaScale + character.manaBonus) * character.manaMult);
    }

    [SerializeField] private float staminaScale = 1;
    public int stamina;
    public int maxStamina
    {
        get => (int)(((character.GetStatPoints()[Stats.Stat.Agi] + 50) * staminaScale + character.staminaBonus) * character.staminaMult);
    }

    #endregion

    private void Awake()
    {
        character = GetComponent<CharacterProfile>();

        FullRestore();
    }

    /// <summary>
    /// Restore health, mana, and stamina to max.
    /// </summary>
    public void FullRestore()
    {
        health = maxHealth;
        mana = maxMana;
        stamina = maxStamina;
    }
}
