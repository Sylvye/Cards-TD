using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    public Deckbuilder DB;
    public Hand hand;
    public static LayerMask placementLayerMask_;
    public LayerMask placementLayerMask;

    // Start is called before the first frame update
    void Start()
    {
        placementLayerMask_ = placementLayerMask;
        DB.InitializeDeck();
        hand.Deal();
        hand.DisplayCards();
    }
}
