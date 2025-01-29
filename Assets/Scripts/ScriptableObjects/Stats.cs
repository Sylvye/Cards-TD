using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats
{
    private Dictionary<string, float> stats = new Dictionary<string, float>();

    public void AddStat(string name, float value)
    {
        stats.Add(name, value);
    }

    public void SetStat(string name, float value)
    {
        stats[name] = value;
    }

    public void AddToStat(string name, float value)
    {
        stats[name] += value;
    }

    public float GetStat(string name)
    {
        return stats[name];
    }

    public void ClearStats()
    {
        stats.Clear();
    }
}
