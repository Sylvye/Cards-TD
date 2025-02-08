using AYellowpaper.SerializedCollections;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Numerics;
using UnityEngine;
using UnityEngine.UIElements;

public class Stats : MonoBehaviour
{
    //private Dictionary<string, float> stats = new();
    [SerializedDictionary("Name", "Value")]
    public SerializedDictionary<string, float> stats = new();

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
        return float.MinValue;
    }

    public int GetLength()
    {
        return stats.Count;
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
            float preVal = other.GetStat(key);

            bool s = other.AddToStat(key, val);
            //if (s)
            //    Debug.Log("[" + name + " -> " + other.name + "] Adding " + val + " to \"" + key + "\" (" + preVal + "), got " + other.GetStat(key));

            if (s) // if we successfully added the stat
            {
                successful = true;
            }
        }

        return successful;
    }

    public override string ToString()
    {
        string message = "";
        for (int i=0; i<stats.Count; i++)
        {
            message += stats.Keys.ElementAt(i) + " = " + stats.Values.ElementAt(i) + "\n";
        }
        return message;
    }
}
