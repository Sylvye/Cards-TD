using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopItem : Button
{
    public Purchaseable item;
    public int price;
    public float discount = 0;

    //SpriteRenderer sr = GetComponent<SpriteRenderer>();
    //sr.sprite = item.GetSprite();

    // CODE ABOVE SETS SPRITE, FIND OUT HOW TO INCORPERATE WITH THE BUTTON CLASS

    // player buys the item
    public override void Action()
    {
        if (Main.currency >= price * discount)
        {
            Main.currency -= price;
            item.Claim();
        }
    }
}
