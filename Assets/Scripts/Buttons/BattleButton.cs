using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BattleButton : Button
{
    Sprite startUp;
    Sprite startDown;
    public Sprite exitUp;
    public Sprite exitDown;

    private void Start()
    {
        startScale = transform.localScale;
        startUp = spriteUp;
        startDown = spriteDown;
    }

    public override void Action()
    {
        if (spriteUp.Equals(startUp))
        {
            if (active)
            {
                Spawner.main.active = true;
                spriteUp = exitUp;
                spriteDown = exitDown;
            }
        }
        else
        {
            if (Spawner.main.IsStageComplete())
            {
                StageController.SwitchStage("Map");
                spriteUp = startUp;
                spriteDown = startDown;
            }
        }
    }
}
