using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeButton : Button
{
    // Update is called once per frame
    void Update()
    {
        SetActive(UpgradeTable.GetCard() != null && Main.playerStats.GetStat("currency") >= UpgradeTable.UpgradeCost());
    }

    public override void Action()
    {
        if (UpgradeTable.Upgrade()) // if successful
        {
            Main.playerStats.ModifyStat("currency", UpgradeTable.UpgradeCost(), Stats.Operation.Subtract);
        }
    }
}
