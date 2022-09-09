using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Defines helper functions for transferring items that are independent from any object.
/// </summary>
public class TransferManager : Singleton<TransferManager>
{
    [SerializeField] private Equipment player;

    protected override void Awake()
    {
        base.Awake();

        player = GameObject.Find("Player").GetComponent<Equipment>();
    }

    /// <summary>
    /// Returns a number to display given an item, the transfer type, and the amount being transferred.
    /// </summary>
    public float GetTransferInfo(Item item, string type, int amt)
    {
        float n = 0;
        Dictionary<Stats.Stat, int> playerStatPoints = player.GetStatPoints();

        switch (type)
        {
            case "Learn":
                n = item.value * amt;
                break;

            case "Buy":
                n = item.value * amt;
                break;

            case "Sell":
                n = (int)(item.value * amt * 0.8f);
                n = (item.value > 0 && n == 0) ? 1 : n;
                break;

            case "Steal":
                if (item.value < 0)
                    n = 0;
                else
                    n = (float)Math.Round((playerStatPoints[Stats.Stat.Dex] + 1) / ((float)(item.value + 1) * amt) * 100, 2); // TWEAK
                break;
        }

        return n;
    }
}
