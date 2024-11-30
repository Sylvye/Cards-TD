using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AugmentLootItem : LootItem
{
    public Augment augment;

    public override void Claim()
    {
        Augment newAugment = Instantiate(augment, Vector3.up * 12, Quaternion.identity);
        Cards.AddToAugments(newAugment);
    }
}
