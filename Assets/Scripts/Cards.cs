using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Cards
{
    public static List<Card> deck = new();
    public static List<Augment> augments = new();

    // picks a random position in the deck to return a card from. The returned card is removed from the deck
    public static Card Draw()
    {
        int index = Random.Range(0, deck.Count);
        Card c = deck[index];
        deck.RemoveAt(index);
        return c;
    }

    public static void Add(Card c)
    {
        c.indexInHand = -1;
        deck.Add(c);
    }
}
