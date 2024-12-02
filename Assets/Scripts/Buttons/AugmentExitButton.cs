using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AugmentExitButton : Button
{
    public override void Action()
    {
        StageController.SwitchStage("Map");
    }
}
