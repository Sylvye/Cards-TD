using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    public static Hand main;
    public List<Card> cards = new();

    // Start is called before the first frame update
    void Start()
    {
        main = this;
    }

    // returns all cards back to the deck. adds 5 cards from the deck to the hand
    public void Deal()
    {
        if (cards.Count > 0)
        {
            for (int i = 0; i < cards.Count; i++)
            {
                Card c = cards[cards.Count-1];
                cards.RemoveAt(cards.Count-1);
                if (c != null)
                    Deck.Add(c);
            }
        }
        for (int i=0; i<5; i++)
        {
            Card c = Deck.Draw();
            cards.Add(c);
            c.indexInHand = i;
        }
    }

    public void DisplayCards()
    {
        int i = 0;
        foreach (Card card in cards)
        {
            card.indexInHand = i;
            card.transform.position = transform.position + Vector3.right * 2 * i++ + Vector3.forward * -7;
            card.SetHandPos();
        }
    }
}
