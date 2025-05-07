using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameObject Lootpool", menuName = "GameObject Lootpool", order = 1)]
public class GameobjectLootpool : ScriptableObject
{
    public List<GameObject> objects;
    public List<float> weights;

    public GameObject GetRandom()
    {
        return objects[WeightedRandom.SelectWeightedIndex(weights)];
    }
}