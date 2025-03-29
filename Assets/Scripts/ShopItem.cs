using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Unity.VisualScripting;
using UnityEngine;

public abstract class ShopItem : Button
{
    public int price;
    private float discount = 0;
    private Purchaseable p;

    public override void Start()
    {
        base.Start();
        p = GetComponent<Purchaseable>();
        spriteUp = p.GetSprite();
        spriteDown = p.GetSprite();
    }

    public override void Action()
    {
        if (Main.playerStats.GetStat("currency") >= GetPrice())
        {
            Main.Earn(-GetPrice());
            p.Claim();
            SetActive(false);
        }
    }

    public int GetPrice()
    {
        return Mathf.RoundToInt(price * 1-GetDiscount());
    }

    public float GetDiscount()
    {
        float disc = Mathf.Max(Main.playerStats.GetStat("base_discount"), discount);
        if (disc > 1) disc = 1;
        // what would this line even do lol
        //else if (disc > 0.9) disc = 0.9f;
        else if (disc < 0) disc = 0;
        return disc;
    }
}
