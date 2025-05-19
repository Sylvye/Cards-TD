using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Hand : MonoBehaviour
{
    public static Hand main;
    private List<Card> hand = new();
    private List<Puppet> puppets = new();
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
        Debug.Log("Hand: "+main.transform.position);
        main.hand.Add(c);
        CardPuppet p = (CardPuppet)c.MakePuppet();
        p.transform.SetParent(main.transform);
        p.transform.localPosition = CalcCardHandPos(main.hand.Count-1);
        p.zPos = -1;
        Debug.Log("Puppet" + p.transform.position);
        main.puppets.Add(p);
    }

    public static int GetIndexOf(Card c)
    {
        return main.hand.IndexOf(c);
    }

    public static Card GetCard(int index)
    {
        return main.hand[index];
    }

    public static Puppet GetPuppet(int index)
    {
        return main.puppets[index];
    }

    public static void Remove(Card c)
    {
        main.hand.RemoveAt(GetIndexOf(c));
        Destroy((CardPuppet)main.puppets[GetIndexOf(c)]);
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
            CardPuppet card = (CardPuppet)GetPuppet(i);
            Vector3 pos = CalcCardHandPos(i);
            card.transform.localPosition = pos;
            card.SetDestination(pos);
            card.zPos = -1;
        }
    }

    public static void ReturnAll()
    {
        foreach (CardPuppet c in main.puppets)
        {
            c.Return();
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

    public static Vector3 CalcCardHandPos(int index)
    {
        return index * 1.5f * Vector3.right + Vector3.back;
    }
}
