using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeTable : Table
{
    public static UpgradeTable main;
    public static int upgrades = 0;

    // Start is called before the first frame update
    void Start()
    {
        main = this;
    }

    public static bool Upgrade() // upgrades a card by one tier. if successful, returns true
    {
        ScrollAreaItemPuppet card = GetCard();
        if (card == null) return false;
        Card c = (Card)card.GetReference();
        if (c.stats.GetStat("tier") >= 5) return false;
        c.stats.ModifyStat("tier", 1);
        c.UpdateSprite();
        card.SetSprite(c.GetSprite());

        return true;
    }

    public static ScrollAreaItemPuppet GetCard()
    {
        return main.transform.GetChild(0).childCount == 0 ? null : main.transform.GetChild(0).GetComponentInChildren<ScrollAreaItemPuppet>();
    }

    // 15(x+T-1)^2
    // x = upgrades
    // T = tier
    public static int UpgradeCost()
    {
        ScrollAreaItemPuppet c = GetCard();
        if (c == null) return -1;
        Card card = (Card)c.GetReference();
        if (card.stats.GetStat("tier") > 4) return -1;
        int tier = (int)card.stats.GetStat("tier");
        return 15 * (int)Mathf.Pow(upgrades + tier - 1, 2);
    }
}
