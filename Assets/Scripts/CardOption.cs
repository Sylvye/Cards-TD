using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardOption : MonoBehaviour
{
    public Card card;
    public LayerMask cardMask;
    public bool active = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMouseUpAsButton()
    {
        if (active)
        {
            Deck.Add(card);
        }
    }

    private void OnMouseUp()
    {
        if (Physics2D.OverlapPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition), cardMask))
        {
            active = false;
            Destroy(gameObject, 1);
        }
    }
}
