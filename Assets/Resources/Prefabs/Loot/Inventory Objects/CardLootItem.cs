using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardLootItem : LootItem
{
    public Card cardReference;
    public int tier;

    public override void OnStart()
    {
        SetSprite(cardReference.GetSprite());
    }

    public override void Claim()
    {
        Card newCard = (Card)cardReference.Clone();
        newCard.stats.SetStat("tier", tier);
        info += tier;
        Cards.AddToDeck(newCard);
    }

    public override int CompareTo(ScrollAreaItem other)
    {
        return base.CompareTo(other);
    }
}
