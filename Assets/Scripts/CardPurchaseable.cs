using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardPurchaseable : ShopItem, Purchaseable
{
    public Card card;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        GetComponent<SpriteRenderer>().sprite = GetSprite();
    }

    public override Sprite GetSprite()
    {
        return card.GetSprite();
    }

    public void Claim()
    {
        Card newCard = Instantiate(card, new Vector3(0, 10, 0), Quaternion.identity);
        Cards.AddToDeck(newCard);
        card.transform.position = new Vector3(0, 10, -2);
    }
}
