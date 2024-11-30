using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEditor.Rendering.CameraUI;

public class Hand : MonoBehaviour
{
    public static Hand main;
    private List<Card> hand = new();

    // Start is called before the first frame update
    private void Start()
    {
        main = this;
    }

    // returns all cards back to the deck. adds 5 cards from the deck to the hand
    public static void Deal()
    {
        if (main.hand.Count > 0) // if there are cards in the hand, safely remove them without deleting them
        {
            for (int i = 0; i < main.hand.Count; i++)
            {
                Card c = main.hand[^1];
                main.hand.RemoveAt(main.hand.Count - 1);
                if (c != null)
                    Cards.AddToDeck(c);
                c.transform.position = Vector3.up * 10;
            }
        }
        for (int i=0; i<5; i++) // draw 5 new cards
        {
            Card c = Cards.DrawFromDeck();
            main.hand.Add(c);
            c.transform.SetParent(main.transform);
        }
    }

    public static void Draw()
    {
        if (Cards.DeckSize() > 0)
        {
            Card c = Cards.DrawFromDeck();
            main.hand.Add(c);
            c.transform.SetParent(main.transform);
            RepositionHand();
        }
    }

    public static void Draw(int num)
    {
        for (int i=0; i<num; i++)
        {
            if (Cards.DeckSize() == 0) break;
            Card c = Cards.DrawFromDeck();
            main.hand.Add(c);
            c.transform.SetParent(main.transform);
        }
        RepositionHand();
    }

    public static void Clear()
    {
        foreach (Card c in main.hand)
        {
            Cards.AddToDeck(c);
            main.hand.Remove(c);
            Cards.AddToDeck(c);
        }
    }

    public static int GetIndexOf(Card c)
    {
        return main.hand.IndexOf(c);
    }

    public static Card Get(int index)
    {
        return main.hand[index];
    }

    public static Card Remove(int index)
    {
        Card c = main.hand[index];
        main.hand.RemoveAt(index);
        Cards.AddToDeck(c);
        return c;
    }

    public static void Remove(Card c)
    {
        main.hand.Remove(c);
        Cards.AddToDeck(c);
    }
    
    public static Card Set(int index, Card c)
    {
        Card output = main.hand[index];
        main.hand[index] = c;
        c.transform.SetParent(main.transform);
        Cards.AddToDeck(output);
        return output;
    }

    public static int Size()
    {
        return main.hand.Count;
    }

    public static void RepositionHand() // moves cards to their spots in the card bar from the stack of cards out of frame
    {
        for (int i=0; i<main.hand.Count; i++)
        {
            Card card = main.hand[i];
            card.transform.position = main.transform.position + 1.2f * i * Vector3.right + Vector3.forward * -5;
            card.SetHandPos();
        }
    }
}
