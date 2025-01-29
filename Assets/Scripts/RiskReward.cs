using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RiskReward : Button
{
    public bool player;
    public GameObject outline;
    public Dictionary<string, float> stats;

    public override void OnMouseEnter()
    {
        base.OnMouseEnter();
        outline.transform.position = transform.position;
        outline.SetActive(true);
    }

    public override void OnMouseExit()
    {
        base.OnMouseExit();
        outline.SetActive(false);
    }e

    public override void Action()
    {
        Stats toEdit = player ? Main.playerStats : Main.enemyStats;
        for (int i = 0; i < stats.Count; i++)
        {
            toEdit.AddToStat(stats.Keys.ElementAt(i), stats[stats.Keys.ElementAt(i)]);
        }
    }
}
