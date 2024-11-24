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
            if (GetActive())
            {
                Spawner.main.active = true;
                spriteUp = exitUp;
                spriteDown = exitDown;
                UpdateSprite();
                SetActive(false);
            }
        }
        else
        {
            if (Spawner.main.IsStageComplete()) // make this open inventory screen
            {
                StageController.SwitchStage("Map");
                spriteUp = startUp;
                spriteDown = startDown;
                UpdateSprite();
            }
        }
    }
}
