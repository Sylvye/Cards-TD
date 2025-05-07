using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Lootpool", menuName = "Stat Item Lootpool", order = 0)]
public class StatItemLootpool : ScriptableObject
{
    public List<ScriptableStatItem> items;
    public List<float> weights;

    public ScriptableStatItem GetRandom()
    {
        return items[WeightedRandom.SelectWeightedIndex(weights)];
    }
}