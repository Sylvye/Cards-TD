using JetBrains.Annotations;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Sprite Lootpool", menuName = "Sprite Lootpool", order = 2)]
public class SpriteLootpool : ScriptableObject
{
    public List<Sprite> sprites;
    public List<float> weights;

    public Sprite GetRandom()
    {
        return sprites[WeightedRandom.SelectWeightedIndex(weights)];
    }
}