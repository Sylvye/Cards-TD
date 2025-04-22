using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AugmentPurchaseable : ShopItem, Purchaseable
{
    public Augment augment;

    // Start is called before the first frame update
    public override void OnStart()
    {
        SetSprite(CalcSprite());
    }

    public Sprite CalcSprite()
    {
        return augment.sprite;
    }

    public void Claim()
    {
        Cards.AddToAugments(augment); // TEMP - MAKE A DEEPCOPY
    }
}
