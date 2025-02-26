using System;
using UnityEngine;

public class BattleButton : Button
{
    public static BattleButton main;
    public static int phase = 0;
    public static int speed = 1;
    public string fileName;
    [NonSerialized] public Sprite playUp;
    [NonSerialized] public Sprite playDown;
    [NonSerialized] public Sprite exitUp;
    [NonSerialized] public Sprite exitDown;
    [NonSerialized] public Sprite nextUp;
    [NonSerialized] public Sprite nextDown;
    private Sprite[] sprites;

    public override void Awake()
    {
        base.Awake();
        main = this;
        sprites = Resources.LoadAll<Sprite>(fileName);
        startScale = transform.localScale;
        playUp = sprites[0];
        playDown = sprites[1];
        exitUp = sprites[2];
        exitDown = sprites[3];
        nextUp = sprites[4];
        nextDown = sprites[5];
    }

    private void Update()
    {
        if (Spawner.main.IsStageCleared() && phase == 1)
        {
            phase++;
            SetSprites(nextUp, nextDown);
            MakeSpriteUp();
        }
    }

    public override void Action()
    {
        switch (phase)
        {
            case 0: // Pressed start
                Spawner.main.StartWave();
                int index = 6 + 2 * (int)Mathf.Log(speed, 2);
                StageController.timeScale = speed;
                SetSprites(sprites[index], sprites[index + 1]);
                MakeSpriteUp();
                //OnMouseExit();
                break;
            case 1: // time dilation
                speed *= 2;
                if (speed > 8)
                    speed = 1;
                int index2 = 6 + 2 * (int)Mathf.Log(speed, 2);
                StageController.timeScale = speed;
                SetSprites(sprites[index2], sprites[index2 + 1]);
                return;
            case 2: // Entered Inventory
                Card.ClearField();
                StageController.ToggleDarken(true);
                StageController.ToggleTime(false);
                StageController.inventoryUI.SetActive(true);
                StageController.inventoryLabels.SetActive(true);
                Main.UpdatePackLabels();
                break;
            case 3: // Entered Boon / Curse stage
                StageController.inventoryUI.SetActive(false);
                StageController.inventoryLabels.SetActive(false);
                StageController.boonCurse.SetActive(true);
                RiskRewardPair.Refresh();
                SetActive(false);
                break;
        }
        phase = ++phase % 4;
    }
}
