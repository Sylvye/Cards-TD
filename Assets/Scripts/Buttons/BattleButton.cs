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
                OnMouseExit();
                SetActive(false);
            }
        }
        else
        {
            if (Spawner.main.IsStageComplete())
            {
                if (!StageController.inventoryOverlay.activeSelf) // opens inventory screen
                {
                    StageController.inventoryOverlay.SetActive(true);
                    StageController.inventoryUI.SetActive(true);
                }
                else
                {
                    StageController.inventoryOverlay.SetActive(false);
                    StageController.inventoryUI.SetActive(false);
                    StageController.SwitchStage("Map");
                    spriteUp = startUp;
                    spriteDown = startDown;
                    UpdateSprite();
                }
            }
        }
    }
}
