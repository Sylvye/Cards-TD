using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeButton : Button
{
    // Update is called once per frame
    void Update()
    {
        SetActive(UpgradeTable.GetCard() != null && Main.currency >= UpgradeTable.UpgradeCost());
    }

    public override void Action()
    {
        Main.currency -= UpgradeTable.UpgradeCost();
        UpgradeTable.Upgrade();
    }
}
