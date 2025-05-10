using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

public class Cards : MonoBehaviour
{
    public enum CardType
    {
        Card,
        Augment
    }
    public static Cards main;
    private List<Card> cards = new();
    private List<Augment> augments = new();

    public StatItemLootpool cardLP;
    public StatItemLootpool augmentLP;

    //private ScrollAreaInventory deckSAI;
    //private ScrollAreaInventory augmentSAI;

    private void Awake()
    {
        main = this;
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
        main.cards.Add(c);
    }

    public static void RemoveFromDeck(Card c)
    {
        main.cards.Remove(c);
    }

    public static Card RemoveFromDeck(int index)
    {
        if (index >= DeckSize())
            return null;
        Card output = main.cards[index];
        main.cards.RemoveAt(index);
        return output;
    }

    public static Card GetFromDeck(int index)
    {
        return main.cards[index];
    }

    public static int IndexOfInDeck(Card c)
    {
        return main.cards.IndexOf(c);
    }

    public static int DeckSize()
    {
        return main.cards.Count;
    }

    public static void AddToAugments(Augment a)
    {
        main.augments.Add(a);
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
