using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class RiskReward : CustomUIElement
{
    public bool boon;
    public GameobjectLootpool lootpool;
    public Dictionary<string, float> stats;
    private GameObject outline;

    public override void Start()
    {
        base.Start();
        outline = transform.GetChild(0).gameObject;
        Debug.Log(outline.name);

        RiskReward obj = lootpool.GetRandom().GetComponent<RiskReward>();
        stats = obj.stats;
        GetComponent<SpriteRenderer>().sprite = obj.GetSprite();
    }

    public void OnMouseEnter()
    {
        outline.SetActive(true);
        transform.parent.GetComponent<RiskRewardPair>().OnMouseEnter();
    }

    public void OnMouseExit()
    {
        outline.SetActive(false);
        transform.parent.GetComponent<RiskRewardPair>().OnMouseExit();
    }

    public override void Action()
    {
        Stats toEdit = boon ? Main.playerStats : Main.enemyStats;
        for (int i = 0; i < stats.Count; i++)
        {
            toEdit.AddToStat(stats.Keys.ElementAt(i), stats[stats.Keys.ElementAt(i)]);
        }
    }
}

