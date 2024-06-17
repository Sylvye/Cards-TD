using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    public Deckbuilder DB;
    public Hand hand;

    // Start is called before the first frame update
    void Start()
    {
        DB.InitializeDeck();
        hand.DisplayCards();
    }
}
