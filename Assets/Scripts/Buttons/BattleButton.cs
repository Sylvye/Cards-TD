using UnityEngine;

public class BattleButton : Button
{
    public static BattleButton main;
    public static int phase = 0;
    private Sprite startUp;
    private Sprite startDown;
    public Sprite nextUp;
    public Sprite nextDown;

    private void Awake()
    {
        main = this;
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
        switch (phase)
        {
            case 0:
                Spawner.main.active = true;
                spriteUp = nextUp;
                spriteDown = nextDown;
                SetSpriteUp();
                OnMouseExit();
                SetActive(false);
                break;
            case 1:
                StageController.inventoryUI.SetActive(true);
                StageController.inventoryLabels.SetActive(true);
                Main.UpdatePackLabels();
                break;
            case 2:
                StageController.inventoryUI.SetActive(false);
                StageController.inventoryLabels.SetActive(false);
                StageController.boonCurse.SetActive(true);
                RiskRewardPair.Refresh();
                SetActive(false);
                break;
        }
        phase = ++phase % 3;
    }
}
