using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Hand : MonoBehaviour
{
    public static Hand main;
    private List<Card> hand = new();
    public static float cooldownSum;
    public static float timeOfLastPlay;

    // Start is called before the first frame update
    private void Awake()
    {
        main = this;
        cooldownSum = 0;
        timeOfLastPlay = -999;
    }

    // returns all cards back to the deck. adds 5 cards from the deck to the hand
    public static void Deal()
    {
        Clear();
        for (int i=0; i<5; i++) // draw 5 new cards
        {
            Card c = Cards.DrawFromDeck();
            Add(c);
        }
        RecalculateHand();
    }

    public static void Draw()
    {
        if (Cards.DeckSize() > 0)
        {
            Card c = Cards.DrawFromDeck();
            Add(c);
            RecalculateHand();
        }
    }

    public static void Draw(int num)
    {
        for (int i=0; i<num; i++)
        {
            if (Cards.DeckSize() == 0) break;
            Card c = Cards.DrawFromDeck();
            Add(c);
        }
        RecalculateHand();
    }

    public static void Clear()
    {
        for (int i=Size()-1; i>=0; i--)
        {
            Card c = Get(i);
            Remove(i);
        }
    }

    public static void Add(Card c)
    {
        main.hand.Add(c);
        c.transform.SetParent(main.transform);
        RecalculateHand();
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
        RecalculateHand();
        return c;
    }

    public static void Remove(Card c)
    {
        main.hand.Remove(c);
        Cards.AddToDeck(c);
        RecalculateHand();
    }
    
    public static Card Set(int index, Card c)
    {
        Card output = Get(index);
        main.hand[index] = c;
        c.transform.SetParent(main.transform);
        Cards.AddToDeck(output);
        RecalculateHand();
        return output;
    }

    public static int Size()
    {
        return main.hand.Count;
    }

    public static void RecalculateHand() // moves cards to their spots in the card bar from the stack of cards out of frame
    {
        for (int i=0; i<Size(); i++)
        {
            Card card = Get(i);
            card.transform.localPosition = 2f * i * Vector3.right + Vector3.forward * -5;
            card.SetHandPos();
        }
    }

    public static void ReformatHand()
    {
        foreach (Card c in main.hand)
        {
            c.ReturnToHand();
        }
    }

    // true = show cards; false = hide
    public static void Display(bool b)
    {
        foreach (Transform t in main.transform)
        {
            t.gameObject.SetActive(b);
        }
    }

    // for old system

    //public static void CalculateSum()
    //{
    //    cooldownSum = 0;
    //    foreach (Transform child in main.transform)
    //    {
    //        Card c = child.GetComponent<Card>();
    //        cooldownSum += c.cooldown;
    //    }
    //}
}
