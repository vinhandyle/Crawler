using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Defines the level up menu.
/// </summary>
public class LevelUpMenu : Menu
{
    [Header("Data")]
    [SerializeField] private CharacterProfile profile;
    [SerializeField] private Inventory inventory;   
    [SerializeField] private List<int> deltas;
    private int usedLvls;

    [Header("Menu")]
    [SerializeField] private RadarChart rc;
    [SerializeField] private Text availableLvls;
    [SerializeField] private List<Button> statBtns;
    [SerializeField] private Button confirmBtn;

    protected override void Awake()
    {
        base.Awake();

        for (int i = 0; i < statBtns.Count; ++i)
        {
            int n = i;
            statBtns[n].onClick.AddListener(() => { UpdateStatLevel(n, 1); });
            statBtns[n].gameObject.AddComponent<RightClick>().onRightClick += () => { UpdateStatLevel(n, -1); };
        }

        confirmBtn.onClick.AddListener(
            () =>
            {
                SaveLevelChanges();
                Close();
            }
        );
    }

    public override void Load()
    {
        deltas = new List<int>() { 0, 0, 0, 0, 0, 0 };
        usedLvls = 0;

        availableLvls.text = string.Format("Stat Points Left: {0}", inventory.availableLvls - profile.level);
    }

    #region Button Setup

    /// <summary>
    /// Update the specified stat as a pending change and 
    /// display it as a temporary radar chart for comparison.
    /// </summary>
    private void UpdateStatLevel(int index, int delta)
    {        
        if (profile.level + usedLvls + delta <= inventory.availableLvls && deltas[index] + delta >= 0)
        {
            deltas[index] += delta;
            usedLvls += delta;
            availableLvls.text = string.Format("Stat Points Left: {0}", inventory.availableLvls - profile.level - usedLvls);
            rc.GenerateTempChart(deltas);
        }
    }

    /// <summary>
    /// Save all pending changes to the character's base stats.
    /// </summary>
    private void SaveLevelChanges()
    {
        List<int> statPoints = new List<int>();
        List<int> baseStatPoints = profile.GetBaseStatPointValues();

        for (int i = 0; i < baseStatPoints.Count; ++i)
        {
            statPoints.Add(baseStatPoints[i] + deltas[i]);
        }

        profile.SetBaseStats(statPoints);
        rc.SetValues(profile.GetStatPointValues());
        rc.GenerateChart();
    }

    #endregion
}
