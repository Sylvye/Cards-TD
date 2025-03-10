using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AugmentPurchaseable : ShopItem, Purchaseable
{
    public Augment augment;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        GetComponent<SpriteRenderer>().sprite = GetSprite();
    }

    public override Sprite GetSprite()
    {
        return augment.GetComponent<SpriteRenderer>().sprite;
    }

    public void Claim()
    {
        Augment newAug = Instantiate(augment, new Vector3(-2, 10, 0), Quaternion.identity);
        Cards.AddToAugments(newAug);
        augment.transform.position = new Vector3(-2, 10, -2);
    }
}
