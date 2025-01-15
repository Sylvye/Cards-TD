using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

public class Cards : MonoBehaviour
{
    public static Cards main;
    private List<Card> deck = new();
    private List<Augment> augments = new();

    private void Start()
    {
        main = this;
    }

    public static void AddAllChildren() // loops through children and adds cards/augments to their respective lists if found
    {
        if (main == null)
        {
            main = GameObject.Find("Cards").GetComponent<Cards>();
        }
        for (int i=0; i<main.transform.childCount; i++)
        {
            Transform t = main.transform.GetChild(i);
            if (t.TryGetComponent(out Card c))
            {
                AddToDeck(c);
            }

            if (t.TryGetComponent(out Augment a))
            {
                AddToAugments(a);
            }
        }
    }

    // picks a random position in the deck to return a card from. The returned card is removed from the deck
    public static Card DrawFromDeck()
    {
        int index = Random.Range(0, DeckSize());
        Card c = RemoveFromDeck(index);
        return c;
    }

    public static void AddToDeck(Card c)
    {
        main.deck.Add(c);
        c.transform.SetParent(main.transform);
        c.transform.position = Vector3.up * 10;
    }

    public static void RemoveFromDeck(Card c)
    {
        main.deck.Remove(c);
    }

    public static Card RemoveFromDeck(int index)
    {
        if (index >= DeckSize())
            return null;
        Card output = main.deck[index];
        main.deck.RemoveAt(index);
        return output;
    }

    public static Card GetFromDeck(int index)
    {
        return main.deck[index];
    }

    public static int IndexOfInDeck(Card c)
    {
        return main.deck.IndexOf(c);
    }

    public static int DeckSize()
    {
        return main.deck.Count;
    }

    public static void AddToAugments(Augment a)
    {
        main.augments.Add(a);
        a.transform.SetParent(main.transform);
        a.transform.position = Vector3.up * 12;
    }

    public static void RemoveFromAugments(Augment a)
    {
        main.augments.Remove(a);
    }

    public static Augment RemoveFromAugments(int index)
    {
        Augment output = main.augments[index];
        main.augments.RemoveAt(index);
        return output;
    }

    public static Augment GetFromAugments(int index)
    {
        return main.augments[index];
    }

    public static int IndexOfInAugments(Augment c)
    {
        return main.augments.IndexOf(c);
    }

    public static int AugmentSize()
    {
        return main.augments.Count;
    }
}
