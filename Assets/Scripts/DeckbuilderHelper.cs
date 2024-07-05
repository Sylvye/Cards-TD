using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeckbuilderHelper : MonoBehaviour
{
    public static DeckbuilderHelper main;
    public CardProbs cardProbabilities;
    public GameObject dummyCard;
    public static bool cardSelected = false;

    private void Start()
    {
        main = this;
    }

    public void SetupOptions()
    {
        for (int i = 0; i < 3; i++)
        {
            GameObject Card = Instantiate(dummyCard, new Vector2(-5 + i * 5, -9), Quaternion.identity);
            CardOption cardOption = Card.GetComponent<CardOption>();
            cardOption.card = cardProbabilities.GetRandom();
            Card.GetComponent<SpriteRenderer>().sprite = cardOption.card.gameObject.GetComponent<SpriteRenderer>().sprite;
        }
    }
}
