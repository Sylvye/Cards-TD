using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AugmentLootItem : LootItem
{
    public Augment augment;
    private Augment newAugment;

    public override void Claim()
    {
        newAugment = Instantiate(augment, Vector3.up * 12, Quaternion.identity);
        Cards.AddToAugments(augment);
    }
}
