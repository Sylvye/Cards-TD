using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardPurchaseable : ShopItem, Purchaseable
{
    public Card card;
    public int tier;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        GetComponent<SpriteRenderer>().sprite = GetSprite();
    }

    public override Sprite GetSprite()
    {
        return card.GetSprite(tier);
    }

    public void Claim()
    {
        Card newCard = Instantiate(card, new Vector3(0, 10, 0), Quaternion.identity);
        newCard.stats.SetStat("tier", tier);
        Cards.AddToDeck(newCard);
        card.transform.position = new Vector3(0, 10, -2);

        // adds the card to the scroll area by refreshing the whole inventory
        ShopController.shopScrollArea.DeleteInventory();
        ShopController.shopScrollArea.FillWithCards(StageController.main.cardSAI, StageController.main.transform, 0, Cards.CardType.Card);
    }
}
