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


    public static void Upgrade() // upgrades a card by one tier
    {
        upgrades++;
        ScrollAreaItemCard card = GetCard();
        if (card == null) return;
        Card c = card.prefabReference.GetComponent<Card>();

        c.tier++;
    }

    public static ScrollAreaItemCard GetCard()
    {
        return main.transform.GetChild(0).childCount == 0 ? null : main.transform.GetChild(0).GetComponentInChildren<ScrollAreaItemCard>();
    }

    public static int UpgradeCost()
    {
        ScrollAreaItemCard c = GetCard();
        if (c == null) return -1;
        int tier = c.prefabReference.GetComponent<Card>().tier;
        return 5 * (int)Mathf.Pow(tier + upgrades, 2);
    }
}
