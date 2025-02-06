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

    //SpriteRenderer sr = GetComponent<SpriteRenderer>();
    //sr.sprite = item.GetSprite();

    // CODE ABOVE SETS SPRITE, FIND OUT HOW TO INCORPERATE WITH THE BUTTON CLASS

    // player buys the item

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
            Main.playerStats.AddToStat("currency", -GetPrice());
            p.Claim();
            SetActive(false);
        }
    }

    public int GetPrice()
    {
        return Mathf.RoundToInt(price * GetDiscount());
    }

    public float GetDiscount()
    {
        return Mathf.Max(1-Main.playerStats.GetStat("base_discount"), 1-discount);
    }
}
