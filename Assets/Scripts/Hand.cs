using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Hand : MonoBehaviour
{
    public static Hand main;
    private List<Card> hand = new();
    private List<CardPuppet> puppets = new();
    public static float timeOfLastPlay;

    // Start is called before the first frame update
    private void Awake()
    {
        main = this;
        timeOfLastPlay = -999;
    }

    // returns all cards back to the deck. adds 5 cards from the deck to the hand
    public static void Deal()
    {
        Clear();
        for (int i=0; i<5; i++) // draw 5 new cards
        {
            Card c = Cards.DrawFromDeck();
            AddCard(c);
        }
        RepositionHand();
    }

    public static void Draw()
    {
        if (Cards.DeckSize() > 0)
        {
            Card c = Cards.DrawFromDeck();
            AddCard(c);
            RepositionHand();
        }
    }

    public static void Clear()
    {
        foreach (Card c in main.hand)
        {
            Remove(c);
        }
    }

    public static void AddCard(Card c)
    {
        main.hand.Add(c);
        CardPuppet cp = MakePuppet(c);
        main.puppets.Add(cp);
        cp.transform.SetParent(main.transform);
        RepositionHand();
    }

    public static int GetIndexOf(Card c)
    {
        return main.hand.IndexOf(c);
    }

    public static Card GetCard(int index)
    {
        return main.hand[index];
    }

    public static CardPuppet GetPuppet(int index)
    {
        return main.puppets[index];
    }

    public static void Remove(Card c)
    {
        main.hand.Remove(c);
        main.puppets.RemoveAt(GetIndexOf(c));
        Cards.AddToDeck(c);
        RepositionHand();
    }

    public static int Size()
    {
        return main.hand.Count;
    }

    public static void RepositionHand() // moves cards to their spots in the card bar from the stack of cards out of frame
    {
        for (int i=0; i<Size(); i++)
        {
            CardPuppet card = GetPuppet(i);
            card.SetHandPos();
        }
    }

    public static void ReturnAll()
    {
        foreach (CardPuppet c in main.puppets)
        {
            c.ReturnToHand();
        }
    }

    public static bool CheckForComplete()
    {
        foreach (CardPuppet cp in main.puppets)
        {
            if (cp.IsOffCooldown()) return true;
        }
        return false;
    }
}
