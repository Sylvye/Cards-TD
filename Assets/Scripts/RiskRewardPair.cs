using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RiskRewardPair : Button
{
    public static RiskRewardPair[] riskRewards;
    public Sprite outline;

    private void Awake()
    {
        riskRewards = new RiskRewardPair[2];
    }

    public override void Start()
    {
        base.Start();
        if (riskRewards[0] == null)
            riskRewards[0] = this;
        else
            riskRewards[1] = this;
    }

    public override void OnMouseEnter()
    {
        base.OnMouseEnter();
        GetComponent<SpriteRenderer>().sprite = outline;
    }

    public override void OnMouseExit()
    {
        if (!GetComponent<Collider2D>().OverlapPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition))) // checks if the mouse has actually left the bounds, and not just hovered over another collider
        {
            base.OnMouseExit();
            GetComponent<SpriteRenderer>().sprite = spriteUp;
        }
    }

    public override void Action()
    {
        for (int i=0; i<transform.childCount; i++)
        {
            GameObject obj = transform.GetChild(i).gameObject;
            RiskReward rr = obj.GetComponent<RiskReward>();
            rr.Action();
        }

        // switches away from battle scene
        StageController.inventoryUI.SetActive(false);
        StageController.inventoryLabels.SetActive(false);
        StageController.SwitchStage(StageController.Stage.Map);
        SetSpriteUp();
        Hand.Clear();
        StageController.inventoryLootScrollArea.ClearClaimed();
        StageController.boonCurse.SetActive(false);
        BattleButton.main.spriteUp = BattleButton.main.startUp;
        BattleButton.main.spriteDown = BattleButton.main.startDown;
    }

    public static void Refresh()
    {
        riskRewards[0].transform.GetChild(0).GetComponent<RiskReward>().Refresh();
        riskRewards[0].transform.GetChild(1).GetComponent<RiskReward>().Refresh();
        riskRewards[1].transform.GetChild(0).GetComponent<RiskReward>().Refresh();
        riskRewards[1].transform.GetChild(1).GetComponent<RiskReward>().Refresh();
    }
}
