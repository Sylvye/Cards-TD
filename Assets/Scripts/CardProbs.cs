using JetBrains.Annotations;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Probabilities", menuName = "Card Probability", order = 1)]
public class CardProbs : ScriptableObject
{
    public List<Card> cards;
    public List<float> probabilities;

    public Card GetRandom()
    {
        return cards[WeightedRandom.SelectWeightedIndex(probabilities)];
    }
}