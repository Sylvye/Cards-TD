using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionButton : Button
{
    public static ActionButton main;

    private void Awake()
    {
        main = this;
    }

    private void Update()
    {
        SetActive(Hand.Size() > 0);
    }

    public override void Action()
    {
        StageController.ToggleDarken(true);
        StageController.ToggleTime(false);
        SetActive(false);

        Hand.Display(true);
    }
}
