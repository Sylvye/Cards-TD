using System;
using System.Collections;
using System.Collections.Generic;
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
        return false;
    }

    // returns min_value if failed
    public float GetStat(string name)
    {
        if (stats.TryGetValue(name, out float val))
        {
            return val;
        }
        return Int32.MinValue;
    }

    public void ClearStats()
    {
        stats.Clear();
    }

    public void AddToStats(Stats other)
    {
        for (int i=0; i<stats.Count; i++)
        {
            string key = stats.ElementAt(i).Key;
            float val = stats.ElementAt(i).Value;
            if (other.GetStat(key) != Int32.MinValue && key.Substring(0, 5).Equals("mult_")) { // multiply if there is not an exact match and it starts with mult_
                other.SetStat(key.Substring(5), other.GetStat(key) * val); // multiplies the key in other (without the mult_) by val (ex. mult_speed would multiply speed by its value) 
            }
            else // add
            {
                other.AddToStat(key, val);
            }
        }
    }
}
