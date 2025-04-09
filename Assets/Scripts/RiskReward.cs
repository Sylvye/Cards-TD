using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Json;
using Unity.VisualScripting;
using UnityEngine;

public class RiskReward : SpriteUIE
{
    public static RiskReward lastBoon;
    public static RiskReward lastCurse;
    public bool boon;
    public bool clone = false;
    public string description;
    public List<string> statNames;
    public List<float> statValues;
    public GameobjectLootpool lootpool;
    private GameObject outline;
    private Textbox textbox;
    private RiskReward prefab;

    public override void OnAwake()
    {
        textbox = GameObject.Find("Risk-Reward Textbox").GetComponent<Textbox>();
        outline = transform.GetChild(0).gameObject;
        Refresh();
    }

    public void Refresh()
    {
        prefab = lootpool.GetRandom().GetComponent<RiskReward>();
        statNames = prefab.statNames;
        statValues = prefab.statValues;
        description = prefab.description;
        clone = prefab.clone;
        SetSprite(prefab.GetSprite());

        // for x2 and x3
        if (clone)
        {
            RiskReward last = boon ? lastBoon : lastCurse;
            if (last == null) // if there hasn't been a boon/curse selected yet
            {
                Refresh();
            }
            else
            {
                statNames = last.statNames;
                statValues = last.statValues;
                description += "\n\n" + last.description;
            }
        }
    }

    public void OnMouseEnter()
    {
        outline.SetActive(true);
        transform.parent.GetComponent<RiskRewardPair>().OnMouseEnter();

        Vector3 XOffset = boon ? Vector2.left : Vector2.right;
        Vector3 YOffset = transform.position.y > 0 ? Vector2.down : Vector2.up;
        Vector3 textPos = transform.position + XOffset * 5 + YOffset;

        textbox.transform.position = textPos;
        textbox.text.SetText(description);
        textbox.gameObject.SetActive(true);
    }

    public void OnMouseExit()
    {
        outline.SetActive(false);
        transform.parent.GetComponent<RiskRewardPair>().OnMouseExit();
        textbox.gameObject.SetActive(false);
    }

    public void OnMouseDown()
    {
        transform.parent.GetComponent<RiskRewardPair>().OnMouseDown();
    }

    private void OnMouseUpAsButton()
    {
        transform.parent.GetComponent<RiskRewardPair>().OnMouseUpAsButton();
    }

    public void Action()
    {
        if (!clone)
        {
            if (boon)
            {
                lastBoon = prefab;
            }
            else
            {
                lastCurse = prefab;
            }
        }
        Stats toEdit = boon ? Main.playerStats : Main.enemyStats;
        for (int i = 0; i < statNames.Count; i++)
        {
            float val = statValues[i];
            if (clone)
            {
                val *= boon ? 2 : 3;
            }
            toEdit.ModifyStat(statNames[i], val);
        }
    }
}

