using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckbuilderHelper : MonoBehaviour
{
    public CardProbs cardProbabilities;
    public GameObject dummyCard;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 3; i++)
        {
            GameObject Card = Instantiate(dummyCard, new Vector2(-5 + i * 5, 0), Quaternion.identity);
            CardOption cardOption = Card.GetComponent<CardOption>();
            cardOption.card = cardProbabilities.GetRandom();
            Card.GetComponent<SpriteRenderer>().sprite = cardOption.card.gameObject.GetComponent<SpriteRenderer>().sprite;
        }
    }
}
