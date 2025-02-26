using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionButton : Button
{
    public static ActionButton main;
    public static bool active = false;

    public override void Awake()
    {
        base.Awake();
        main = this;
    }

    private void Update()
    {
        SetActive(Hand.Size() > 0 && (BattleButton.phase == 0 || !Spawner.main.IsStageCleared()));
    }

    public override void Action()
    {
        active = !active;

        if (active)
        {
            StageController.ToggleDarken(true);
            StageController.ToggleTime(false);

            Hand.ReformatHand();
            Hand.Display(true);
        }
        else
        {
            StageController.ToggleDarken(false);
            StageController.ToggleTime(true);

            Hand.Display(false);
        }
    }
}
