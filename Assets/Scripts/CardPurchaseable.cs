using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardPurchaseable : ShopItem, Purchaseable
{
    public Card card;
    public int tier;

    // Start is called before the first frame update
    public override void OnStart()
    {
        GetComponent<SpriteRenderer>().sprite = CalcSprite();
    }

    public Sprite CalcSprite()
    {
        return card.CalcSprite(tier);
    }

    public void Claim()
    {
        Card newCard = (Card)card.Clone();
        newCard.stats.SetStat("tier", tier);
        Cards.AddToDeck(newCard);

        // adds the card to the scroll area by refreshing the whole inventory
        ShopController.shopScrollArea.DeleteInventory();
        ShopController.shopScrollArea.FillWithCards(StageController.main.cardSAI, StageController.main.transform, 0, Cards.CardType.Card);
    }
}
