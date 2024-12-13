using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AugmentTable : MonoBehaviour
{
    public static AugmentTable main;

    // Start is called before the first frame update
    void Start()
    {
        main = this;        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return)) // if enter key pressed, merge. TEMPORARY UNTIL BUTTON IS IMPLEMENTED
        {
            Merge();
        }
    }

    public static void Merge() // combines an augment with a card, destroying the augment
    {
        AugmentSceneScrollAreaItem augment = main.transform.GetChild(0).GetComponentInChildren<AugmentSceneScrollAreaItem>();
        AugmentSceneScrollAreaItem card = main.transform.GetChild(1).GetComponentInChildren<AugmentSceneScrollAreaItem>();
        Augment a = augment.reference.GetComponent<Augment>();
        TowerCard c = card.reference.GetComponent<TowerCard>();

        a.ApplyEffect(c);
        Cards.RemoveFromAugments(a);
        Destroy(augment.gameObject);
        Destroy(a.gameObject);
    }
}
