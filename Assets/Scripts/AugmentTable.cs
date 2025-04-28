using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class AugmentTable : Table
{
    public static AugmentTable main;

    // Start is called before the first frame update
    void Start()
    {
        main = this;        
    }

    public static void Merge() // combines an augment with a card, destroying the augment
    {
        // TEMP
        ScrollAreaItemCard augmentSAI = main.transform.GetChild(0).GetComponentInChildren<ScrollAreaItemCard>();
        ScrollAreaItemCard cardSAI = main.transform.GetChild(1).GetComponentInChildren<ScrollAreaItemCard>();
        Augment a = augmentSAI.reference.GetComponent<Augment>();
        TowerCard c = cardSAI.reference.GetComponent<TowerCard>();

        a.ApplyEffect(c);
        Cards.RemoveFromAugments(a);
        Destroy(augmentSAI.gameObject);
        Destroy(ap.gameObject);
    }
}
