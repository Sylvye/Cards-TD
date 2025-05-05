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
        ScrollAreaItemPuppet augmentSAI = main.transform.GetChild(0).GetComponentInChildren<ScrollAreaItemPuppet>();
        ScrollAreaItemPuppet cardSAI = main.transform.GetChild(1).GetComponentInChildren<ScrollAreaItemPuppet>();
        Augment a = (Augment)augmentSAI.GetReference();
        Card c = (Card)cardSAI.GetReference();

        a.ApplyEffect(c);
        Cards.RemoveFromAugments(a);

        // destroy physical puppet
        Destroy(augmentSAI.gameObject);
    }
}
