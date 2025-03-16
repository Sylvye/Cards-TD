using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeButton : Button
{
    private TMPLabel costLabel;

    public override void Awake()
    {
        base.Awake();
        costLabel = GameObject.Find("Cost Label").GetComponent<TMPLabel>();
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
        int cost = UpgradeTable.UpgradeCost();
        // card isnt null, tier < 5, and can afford
        SetActive(UpgradeTable.GetCard() != null && UpgradeTable.GetCard().prefabReference.GetComponent<Card>().stats.GetStat("tier") < 5 && Main.playerStats.GetStat("currency") >= cost);
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
            Main.playerStats.ModifyStat("currency", cost, Stats.Operation.Subtract);
            UpgradeTable.upgrades++;
        }
    }
}
