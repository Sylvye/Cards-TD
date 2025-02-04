using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RiskRewardPair : Button
{
    private Textbox textbox;


    public override void Start()
    {
        base.Start();
        textbox = GameObject.Find("Risk-Reward Textbox").GetComponent<Textbox>();
    }

    public override void OnMouseEnter()
    {
        base.OnMouseEnter();
        GetComponent<SpriteRenderer>().sprite = spriteDown;
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
    }
}
