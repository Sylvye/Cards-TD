using JetBrains.Annotations;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Probabilities", menuName = "Probability List", order = 1)]
public class Lootpool : ScriptableObject
{
    public List<GameObject> objects;
    public List<float> weights;

    public GameObject GetRandom()
    {
        return objects[WeightedRandom.SelectWeightedIndex(weights)];
    }
}