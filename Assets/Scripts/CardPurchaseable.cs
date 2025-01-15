using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardPurchaseable : MonoBehaviour, Purchaseable
{
    public Card card;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public Sprite GetSprite()
    {
        return card.GetSprite();
    }

    public void Claim()
    {
        Cards.AddToDeck(card);
        card.transform.position = new Vector3(0, 10, -2);
    }
}
