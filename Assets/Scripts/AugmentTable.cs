using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AugmentTable : MonoBehaviour
{
    public static AugmentTable main;
    public GameObject outputSlot;

    // Start is called before the first frame update
    void Start()
    {
        main = this;        
    }

    // Update is called once per frame
    void Update()
    {
        if (StageController.stageIndex == 3 && transform.childCount == 5)
        {
            Merge();
        }
    }

    public static void Merge() // combines an augment with a card, destroying the augment
    {
        Card c = main.GetComponentInChildren<Card>();
        Augment a = main.GetComponentInChildren<Augment>();

        c.transform.position = main.outputSlot.transform.position;
        a.ApplyEffect(c);
        Cards.RemoveFromAugments(a);
        Destroy(a.gameObject);
    }
}
