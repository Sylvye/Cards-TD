using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Numerics;
using UnityEngine;
using UnityEngine.UIElements;

public class Stats
{
    private Dictionary<string, float> stats = new();

    // returns true if successful
    public bool AddStat(string name, float value)
    {
        return stats.TryAdd(name, value);
    }

    // returns true if successful
    public bool SetStat(string name, float value)
    {
        if (stats.TryGetValue(name, out _))
        {
            stats[name] = value;
            return true;
        }
        Debug.LogWarning("Couldn't SET stat \"" + name + "\"");
        return false;
    }

    // returns true if successful
    public bool AddToStat(string name, float value)
    {
        if (stats.TryGetValue(name, out _))
        {
            stats[name] += value;
            return true;
        }
        Debug.LogWarning("Couldn't ADD stat \"" + name + "\"");
        return false;
    }

    // returns min_value if failed
    public float GetStat(string name)
    {
        if (stats.TryGetValue(name, out float val))
        {
            return val;
        }
        Debug.LogWarning("Couldn't GET stat \"" + name + "\"");
        return Int32.MinValue;
    }

    public void ClearStats()
    {
        stats.Clear();
    }

    public bool AddToStats(Stats other)
    {
        bool successful = false;
        for (int i=0; i<stats.Count; i++)
        {
            string key = stats.ElementAt(i).Key;
            float val = stats.ElementAt(i).Value;
            // if other doesnt contain the key, and the key starts with "mult_", then multiply instead of adding 
            if (!other.stats.ContainsKey(key) && key.Length > 5 && key.Substring(0, 5).Equals("mult_")) { // multiply if there is not an exact match and it starts with mult_
                string newKey = key.Substring(5);
                float preVal = other.GetStat(newKey);
                other.SetStat(newKey, other.GetStat(newKey) * val); // multiplies the key in other (without the mult_) by val (ex. mult_speed would multiply speed by its value) 
                Debug.Log("Multiplying \"" + newKey + "\" (" + preVal + ") by " + val + " to get " + other.GetStat(newKey));
            }
            else // add
            {
                other.AddToStat(key, val);
                Debug.Log("Adding " + val + " to \"" + key + "\", got " + other.GetStat(key));
            }

            if (!successful && stats.ContainsKey(key))
            {
                successful = true;
            }
        }

        return successful;
    }
}
