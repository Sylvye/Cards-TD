using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeButton : Button
{
    private TMPLabel costLabel;

    public override void OnAwake()
    {
        costLabel = GameObject.Find("Cost Label").GetComponent<TMPLabel>();
    }

    // Update is called once per frame
    public override void OnUpdate()
    {
        int cost = UpgradeTable.UpgradeCost();
        // card isnt null, tier < 5, and can afford
        SetClickable(UpgradeTable.GetCard() != null && ((Card)UpgradeTable.GetCard().GetReference()).stats.GetStat("tier") < 5 && Main.playerStats.GetStat("currency") >= cost);
        string text = "";
        if (cost == 0)
        {
            text = "free!";
        }
        else if (cost > 0)
        {
            text = cost + "c";
        }

        costLabel.SetText(text);
    }

    public override void Action()
    {
        float cost = UpgradeTable.UpgradeCost();
        if (UpgradeTable.Upgrade()) // if successful
        {
            Main.Earn((int)-cost);
            UpgradeTable.upgrades++;
        }
    }
}
