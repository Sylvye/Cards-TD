using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Deck
{
    public static List<Card> cards = new();

    // picks a random position in the deck to return a card from. The returned card is removed from the deck
    public static Card Draw()
    {
        int index = Random.Range(0, cards.Count);
        Card c = cards[index];
        cards.RemoveAt(index);
        return c;
    }

    public static void Add(Card c)
    {
        c.indexInHand = -1;
        cards.Add(c);
    }
}
