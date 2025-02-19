using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeTable : MonoBehaviour
{
    public static UpgradeTable main;
    public static int upgrades = 0;

    // Start is called before the first frame update
    void Start()
    {
        main = this;
    }


    public static bool Upgrade() // upgrades a card by one tier
    {
        upgrades++;
        ScrollAreaItemCard card = GetCard();
        if (card == null) return false;
        Card c = card.prefabReference.GetComponent<Card>();
        if (c.stats.GetStat("tier") >= 5) return false;
        c.stats.ModifyStat("tier", 1);
        card.SetSprite(c.GetSprite());

        return true;
    }

    public static ScrollAreaItemCard GetCard()
    {
        return main.transform.GetChild(0).childCount == 0 ? null : main.transform.GetChild(0).GetComponentInChildren<ScrollAreaItemCard>();
    }

    public static int UpgradeCost()
    {
        ScrollAreaItemCard c = GetCard();
        if (c == null) return -1;
        int tier = (int)c.prefabReference.GetComponent<Card>().stats.GetStat("tier");
        return 5 * (int)Mathf.Pow(tier + upgrades, 2);
    }
}
