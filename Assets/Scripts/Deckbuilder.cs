using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deckbuilder : MonoBehaviour
{
    public List<Card> cards;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void InitializeDeck()
    {
        foreach (Card card in cards)
        {
            Deck.Add(card);
        }
    }
}