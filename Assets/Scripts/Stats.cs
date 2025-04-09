using AYellowpaper.SerializedCollections;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Numerics;
using UnityEngine;
using UnityEngine.UIElements;

public class Stats : SerializedDictionary<string, Stat>
{
    // returns true if successful
    public bool AddStat(string name, float value, string min, string max)
    {
        return TryAdd(name, new Stat(value, min, max));
    }

    // returns true if successful
    public bool SetStat(string name, float value)
    {
        if (TryGetValue(name, out Stat s))
        {
            s.SetValue(value);
            return true;
        }
        Debug.LogWarning("Couldn't SET stat \"" + name + "\"");
        return false;
    }

    public void SetStatsFromDict(Dictionary<string, Stat> s)
    {
        for (int i = 0; i < s.Count; i++)
        {
            Add(s.ElementAt(i).Key, s.ElementAt(i).Value);
        }
    }

    /// <summary>
    /// Adds value to name
    /// </summary>
    /// <param name="name"></param>
    /// <param name="value"></param>
    /// <returns>True if successful, false otherwise</returns>
    public bool ModifyStat(string name, float value)
    {
        if (TryGetValue(name, out Stat s))
        {
            s.Modify(value, Stat.Operation.Add);
            return true;
        }
        Debug.LogWarning("Couldn't ADD stat \"" + name + "\"");
        return false;
    }

    /// <summary>
    /// Modifies a value
    /// </summary>
    /// <param name="name"></param>
    /// <param name="value"></param>
    /// <param name="o"> The operation to perform on value </param>
    /// <returns>True if successful, false otherwise</returns>
    public bool ModifyStat(string name, float value, Stat.Operation o)
    {
        if (TryGetValue(name, out Stat s))
        {
            s.Modify(value, o);
            return true;
        }
        Debug.LogWarning("Couldn't ADD stat \"" + name + "\"");
        return false;
    }

    // returns min_value if failed
    public float GetStat(string name)
    {
        if (ContainsKey(name))
        {
            return this[name].GetValue();
        }
        Debug.LogWarning("Couldn't GET stat \"" + name + "\"");
        return float.MinValue;
    }

    public bool AddStats(Stats other)
    {
        bool successful = false;
        for (int i = 0; i < other.Count; i++)
        {
            string key = other.ElementAt(i).Key;
            float val = other.ElementAt(i).Value.GetValue();
            if (ModifyStat(key, val)) // if we successfully added the stat
            {
                successful = true;
            }
        }

        return successful;
    }

    public bool AddStatsFromDict(Dictionary<string, Stat> s)
    {
        bool successful = false;
        for (int i = 0; i < s.Count; i++)
        {
            string key = s.ElementAt(i).Key;
            float val = s.ElementAt(i).Value.GetValue();
            if (ModifyStat(key, val)) // if we successfully added the stat
            {
                successful = true;
            }
        }

        return successful;
    }

    public override string ToString()
    {
        string message = "";
        for (int i = 0; i < Count; i++)
        {
            message += Keys.ElementAt(i) + " = " + Values.ElementAt(i).GetValue() + "\n";
        }
        return message;
    }
}
