using JetBrains.Annotations;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item Lootpool", menuName = "Item Lootpool", order = 2)]
public class ItemLootpool : ScriptableObject
{
    public List<LootItem> items;
    public List<float> weights;

    public LootItem GetRandom()
    {
        return items[WeightedRandom.SelectWeightedIndex(weights)];
    }
}