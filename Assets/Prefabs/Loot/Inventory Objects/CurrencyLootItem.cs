using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrencyLootItem : LootItem
{
    public int amount;

    public override void Claim()
    {
        Main.Earn(amount);
    }
}
