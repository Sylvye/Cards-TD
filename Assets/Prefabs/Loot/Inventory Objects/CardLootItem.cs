using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardLootItem : LootItem
{
    public Card cardReference;
    public int tier;

    private void Start()
    {
        GetComponent<SpriteRenderer>().sprite = Resources.LoadAll<Sprite>("CardPack")[cardReference.towerIndex * 5 + tier - 1];
    }

    public override void Claim()
    {
        Card newCard = Instantiate(cardReference, Vector3.up * 10, Quaternion.identity);
        newCard.tier = tier;
        id += tier;
        Cards.AddToDeck(newCard);
    }
}
