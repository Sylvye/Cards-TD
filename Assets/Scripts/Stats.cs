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
    public enum Operation
    {
        Add,
        Subtract,
        Multiply,
        Divide
    }
    [SerializedDictionary("Name", "Value")]
    public SerializedDictionary<string, Stat> stats = new();

    // returns true if successful
    public bool AddStat(string name, float value, float min, float max)
    {
        return stats.TryAdd(name, new Stat(value, min, max));
    }

    // returns true if successful
    public bool SetStat(string name, float value)
    {
        if (stats.TryGetValue(name, out _))
        {
            stats[name].SetValue(value);
            return true;
        }
        Debug.LogWarning("Couldn't SET stat \"" + name + "\"");
        return false;
    }


    /// <summary>
    /// Adds value to name
    /// </summary>
    /// <param name="name"></param>
    /// <param name="value"></param>
    /// <returns>True if successful, false otherwise</returns>
    public bool ModifyStat(string name, float value)
    {
        if (stats.TryGetValue(name, out _))
        {
            stats[name].Modify(value, Operation.Add);
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
    public bool ModifyStat(string name, float value, Operation o)
    {
        if (stats.TryGetValue(name, out _))
        {
            stats[name].Modify(value, o);
            return true;
        }
        Debug.LogWarning("Couldn't ADD stat \"" + name + "\"");
        return false;
    }

    // returns min_value if failed
    public float GetStat(string name)
    {
        if (stats.ContainsKey(name))
        {
            return stats[name].GetValue();
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

    public bool AddStats(Stats other)
    {
        bool successful = false;
        for (int i=0; i<other.stats.Count; i++)
        {
            string key = other.stats.ElementAt(i).Key;
            float val = other.stats.ElementAt(i).Value.GetValue();

            bool s = ModifyStat(key, val);

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
            message += stats.Keys.ElementAt(i) + " = " + stats.Values.ElementAt(i).GetValue() + "\n";
        }
        return message;
    }
}
