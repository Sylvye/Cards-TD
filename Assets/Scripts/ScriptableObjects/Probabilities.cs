using JetBrains.Annotations;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Probabilities", menuName = "Probability List", order = 1)]
public class Probabilities : ScriptableObject
{
    public List<GameObject> objects;
    public List<float> probabilities;

    public GameObject GetRandom()
    {
        return objects[WeightedRandom.SelectWeightedIndex(probabilities)];
    }
}