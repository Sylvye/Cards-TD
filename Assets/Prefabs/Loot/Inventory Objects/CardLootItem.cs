using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardLootItem : LootItem
{
    public Card cardReference;
    public int tier;
    private Card newCard;

    private void Start()
    {
        newCard = Instantiate(cardReference, Vector3.up*10, Quaternion.identity);
        newCard.tier = tier;
        id += tier;
    }

    public override void Claim()
    {
        Cards.AddToDeck(newCard);
    }
}
