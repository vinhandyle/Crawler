using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Defines the item/spell/tech info UI.
/// </summary>
public class InfoMenu : Menu
{
    [SerializeField] private CharacterProfile player;
    [SerializeField] private Item item;
    [SerializeField] private Equipment equipment;
    [SerializeField] private InfoMenu main;

    [Header("UI")]
    [SerializeField] private int mode;
    [SerializeField] private Text displayInfo;
    [SerializeField] private Scrollbar scrollbar;

    protected override void Awake()
    {
        player = GameObject.Find("Player").GetComponent<CharacterProfile>();
        base.Awake();
    }

    /// <summary>
    /// Returns true if the current item displayed is the specified item.
    /// </summary>
    public bool CompareItem(Item item)
    {
        return this.item == item;
    }

    /// <summary>
    /// Set the specificed item as the one to be displayed.
    /// </summary>
    public void SetItem(Item item, Equipment equipment = null, int mode = 0)
    {
        if (item != null)
        {
            this.item = item;
            this.equipment = (equipment != null) ? equipment : player.GetComponent<Equipment>();
            this.mode = mode;
            Open();
        }
    }

    public override void Close()
    {
        base.Close();
        item = null;
    }

    /// <summary>
    /// Display the information of the current item/spell/tech.
    /// </summary>
    public override void Load()
    {
        // Reset scrollbar
        scrollbar.value = 1;

        // Populate info
        string[] displayTexts = 
            {
                DisplayName(), DisplayRequirements(), DisplayCosts(), DisplayDescription(), DisplayStats(), DisplayEffects()
            };

        displayInfo.text = string.Format(
            "{0}{1}{2}{3}{4}{5}",
            displayTexts[0] + "\n",
            displayTexts[1] + "\n",
            displayTexts[2] == "" ? "" : displayTexts[2] + "\n",
            displayTexts[3] == "" ? "" : displayTexts[3] + "\n",
            displayTexts[4] + "\n",
            displayTexts[5]
            );
    }

    #region Get Info to Display

    private string DisplayName()
    {
        string weaponInfo = "";
        Weapon itemW = item.GetItemClass() == Weapon.GetStaticItemClass() ? (Weapon)item : null;
        if (itemW != null)
        { 
            weaponInfo = string.Format(
            "\n[{0}{1}]",
            itemW.type,
            itemW.tech != null ? string.Format("/{0}", itemW.tech.GetName(player.appraisalLvl)) : ""
            );
        }

        return string.Format(
            "{0}{1}{2}",
            item.GetName(player.appraisalLvl),
            item.stackable ? string.Format(" x{0}", item.quantity) : "",
            weaponInfo
            );
    }   

    private string DisplayRequirements()
    {
        string displayReqs = "";

        Dictionary<Stats.Stat, int> reqs = item.GetRequirements();
        if (reqs != null)
        {
            foreach (KeyValuePair<Stats.Stat, int> kvp in reqs)
            {
                displayReqs += string.Format
                    (
                        "{0} Min:\t<color={2}>{1}</color>\n", 
                        kvp.Key.ToString(), 
                        kvp.Value, 
                        kvp.Value <= equipment.GetStatPoints()[kvp.Key] ? "black" : "red"
                    );
            }
        }
        return displayReqs;
    }

    private string DisplayCosts()
    {
        string displayCosts = "";

        Dictionary<Stats.Gauge, int> useCosts = item.GetUseCosts();
        if (useCosts != null)
        {
            foreach (KeyValuePair<Stats.Gauge, int> kvp in useCosts)
            {
                if (kvp.Value > 0)
                {
                    displayCosts += string.Format("{0} Cost:\t{1}\n", kvp.Key.ToString(), kvp.Value);
                }
            }
        }
        return displayCosts;
    }

    private string DisplayDescription()
    {
        return new MenuHelper().StringWindow(item.GetDescription(player.appraisalLvl), 30);
    }

    private string DisplayStats()
    {
        string displayStats = "";

        Dictionary<Stats.Damage, int> baseStats = item.GetBaseStats();
        if (baseStats != null)
        {
            Dictionary<Stats.Stat, int> statPoints = equipment.GetStatPoints();
            Dictionary<Stats.Damage, int> dmgVals = item.GetStats(
                        statPoints[Stats.Stat.Str],
                        statPoints[Stats.Stat.Dex],
                        statPoints[Stats.Stat.Int]
                    );
            Dictionary<Stats.Damage, int> mainDmgVals = main?.item?.GetStats(
                        statPoints[Stats.Stat.Str],
                        statPoints[Stats.Stat.Dex],
                        statPoints[Stats.Stat.Int]
                    );
            Dictionary<Stats.Damage, Color> dmgColor = new Stats().dmgColor;

            // Spell damage
            if (item.GetItemClass() == Weapon.GetStaticItemClass())
            {
                float spellDmg = ((Weapon)item).GetSpellDamage(statPoints[Stats.Stat.Int]);
                float mainSpellDmg = (main?.item == null) ? 0 : ((Weapon)main.item).GetSpellDamage(statPoints[Stats.Stat.Int]);

                if (spellDmg > 0)
                {
                    int n = spellDmg.CompareTo(mainSpellDmg);
                    string color = (n > 0) ? "green" : (n < 0) ? "red" : "black";

                    displayStats += string.Format
                        (
                        "<color=#68adee>Spll Dmg:</color>\t<color={1}>{0}</color>\n",
                        spellDmg,
                        mode == 0 ? "black" : color
                        );
                }
            }

            // Basic attack damage / defense
            foreach (KeyValuePair<Stats.Damage, int> kvp in baseStats)
            {
                int n = (mainDmgVals == null) ? dmgVals[kvp.Key] : dmgVals[kvp.Key].CompareTo(mainDmgVals[kvp.Key]);
                string color = (n > 0) ? "green" : (n < 0) ? "red" : "black";

                displayStats += string.Format
                    (
                        "<color={4}>{0}</color> {3}:\t<color={2}>{1}</color>",
                        kvp.Key.ToString().Substring(0, 4),
                        kvp.Value,
                        mode == 0 ? "black" : color,
                        item.GetItemClass() == Weapon.GetStaticItemClass() || item.GetItemClass() == Ammo.GetStaticItemClass() ? "Dmg" : "Def",
                        "#" + ColorUtility.ToHtmlStringRGBA(dmgColor[kvp.Key])
                    );

                if (item.GetItemClass() == Weapon.GetStaticItemClass())
                {
                    int scaledStat = ((Weapon)item).GetScaledStats(0, 0, 0)[kvp.Key];
                    displayStats += string.Format
                        (
                            " \t<color={2}>{0} {1}</color>", 
                            scaledStat > 0 ? "+" : (scaledStat < 0 ? "-" : ""), 
                            scaledStat != 0 ? scaledStat.ToString() : "",
                            mode == 0 ? "black" : color
                        );
                }
                displayStats += "\n";
            }
        }
        return displayStats;
    }

    private string DisplayEffects()
    {
        string displayEffects = "";
        Dictionary<Stats.Stat, int> statPoints = player.GetStatPoints();

        foreach (Effect effect in item.GetEffects(statPoints[Stats.Stat.Dex], statPoints[Stats.Stat.Int]))
        {
            displayEffects += effect.GetDescripton() + "\n";
        }
        return new MenuHelper().StringWindow(displayEffects, 31);
    }

    #endregion
}
