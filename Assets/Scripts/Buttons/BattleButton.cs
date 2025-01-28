using UnityEngine;

public class BattleButton : Button
{
    private Sprite startUp;
    private Sprite startDown;
    public Sprite exitUp;
    public Sprite exitDown;

    public override void Start()
    {
        base.Start();
        startScale = transform.localScale;
        startUp = spriteUp;
        startDown = spriteDown;
    }

    private void Update()
    {
        if (Spawner.main.IsStageComplete())
        {
            SetActive(true);
        }
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
                if (!StageController.screenDim.activeSelf) // opens inventory screen
                {
                    StageController.screenDim.SetActive(true);
                    StageController.inventoryUI.SetActive(true);
                    Main.UpdatePackLabels();
                }
                else // leaves battle stage
                {
                    StageController.screenDim.SetActive(false);
                    StageController.inventoryUI.SetActive(false);
                    StageController.SwitchStage(StageController.Stage.Map);
                    spriteUp = startUp;
                    spriteDown = startDown;
                    UpdateSprite();
                    Hand.Clear();
                    StageController.inventoryLootScrollArea.ClearClaimed();
                }
            }
        }
    }
}
