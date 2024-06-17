using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    public List<Card> cards = new();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DisplayCards()
    {
        int i = 0;
        foreach (Card card in cards)
        {
            Debug.Log(card); // crashes because hand is empty
            Debug.Log(card.transform);
            Debug.Log(transform);
            card.transform.position = transform.position + Vector3.right * i++;
            Debug.Log(card.transform.position);
        }
    }
}
