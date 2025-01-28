using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiskReward : Button
{
    public GameObject outline;

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
    }

    public override void Action()
    {
        Debug.Log("clicked");
    }
}
