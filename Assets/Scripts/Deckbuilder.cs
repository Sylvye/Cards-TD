using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deckbuilder : MonoBehaviour
{
    public List<Card> cards;
    // Start is called before the first frame update
    void Start()
    {
        foreach(Card card in cards)
        {
            Deck.cards.Add(card);
        }
    }
}
