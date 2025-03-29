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
        ScrollAreaItemCard augment = main.transform.GetChild(0).GetComponentInChildren<ScrollAreaItemCard>();
        ScrollAreaItemCard card = main.transform.GetChild(1).GetComponentInChildren<ScrollAreaItemCard>();
        Augment a = augment.reference.GetComponent<Augment>();
        TowerCard c = card.reference.GetComponent<TowerCard>();

        a.ApplyEffect(c);
        Cards.RemoveFromAugments(a);
        Destroy(augment.gameObject);
        Destroy(a.gameObject);
    }
}
