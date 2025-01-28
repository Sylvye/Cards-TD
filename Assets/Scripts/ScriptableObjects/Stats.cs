using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Statistics", menuName = "Statistics", order = 2)]
public class Stats : MonoBehaviour
{
    private Dictionary<string, float> stats = new Dictionary<string, float>();

    public void AddStat(string name, float value)
    {
        stats.Add(name, value);
    }

    public void EditStat(string name, float value)
    {
        stats[name] = value;
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
