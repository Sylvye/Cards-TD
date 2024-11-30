using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deckbuilder : MonoBehaviour
{
    public List<GameObject> cards;
    public void InitializeDeck()
    {
        foreach (GameObject card in cards)
        {
            Cards.AddToDeck(card.GetComponent<Card>());
        }
    }
}
