using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AugmentPurchaseable : MonoBehaviour, Purchaseable
{
    public Augment augment;

    // Start is called before the first frame update
    void Start()
    {

    }

    public Sprite GetSprite()
    {
        return augment.GetComponent<SpriteRenderer>().sprite;
    }

    public void Claim()
    {
        Cards.AddToAugments(augment);
        augment.transform.position = new Vector3(-2, 10, -2);
    }
}
