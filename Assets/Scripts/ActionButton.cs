using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionButton : Button
{
    public static ActionButton main;

    public override void Awake()
    {
        base.Awake();
        main = this;
    }

    private void Update()
    {
        SetActive(Hand.Size() > 0 && (BattleButton.phase == 1 || Spawner.main.GetActive()));
    }

    public override void Action()
    {
        StageController.ToggleDarken(true);
        StageController.ToggleTime(false);
        SetActive(false);

        Hand.Display(true);
    }
}
