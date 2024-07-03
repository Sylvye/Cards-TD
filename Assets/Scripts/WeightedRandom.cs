using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class WeightedRandom
{
    public static int SelectWeightedIndex(List<float> weights)
    {
        if (weights.Count == 0) throw new ArgumentException("List is empty");

        float weightSum = 0;

        foreach (float weight in weights)
        {
            weightSum += weight;
        }

        float num = UnityEngine.Random.Range(0, weightSum);
        for (int i = 0; i < weights.Count; i++)
        {
            if (num <= weights[i])
            {
                return i;
            }
            num -= weights[i];
        }

        throw new Exception("Code should not reach this state");
    }
}
