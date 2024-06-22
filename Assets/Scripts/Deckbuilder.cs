using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deckbuilder : MonoBehaviour
{
    public List<Card> cards;
    public void InitializeDeck()
    {
        foreach (Card card in cards)
        {
            Deck.Add(card);
        }
    }
}
