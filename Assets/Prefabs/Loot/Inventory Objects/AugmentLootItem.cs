using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AugmentLootItem : LootItem
{
    public AugmentPuppet augment;

    public override void OnStart()
    {
        info = augment.GetInfo();
    }

    public override void Claim()
    {
        Augment newAugment = new();
        Cards.AddToAugments(newAugment);
    }

    public override int CompareTo(ScrollAreaItem other)
    {
        return base.CompareTo(other);
    }

    public override string GetName()
    {
        return "augment of " + info;
    }
}
