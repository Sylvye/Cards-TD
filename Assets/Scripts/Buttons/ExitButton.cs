using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitButton : Button
{
    public override void Action()
    {
        StageController.SwitchStage(StageController.Stage.Map);
    }
}
