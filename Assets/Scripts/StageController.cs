using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;
using UnityEngine.UIElements;
using static UnityEditor.Timeline.Actions.MenuPriority;

public class StageController : MonoBehaviour
{
    public enum Stage
    {
        None,
        Map,
        Battle,
        Augment,
        Shop,
        Upgrade
    }
    [Header("General")]
    public static StageController main;
    public static Stage currentStage = Stage.Map;
    public BackgroundMat[] stageMaterials; // MAP, BATTLE, AUGMENT, SHOP, UPGRADE
    private static Material BGMat;
    private static float lerpProgress = 1;
    private static Vector3 cameraDestination = new(0, -10, -10);
    private static GameObject darkenOverlay;
    public static float timeScale = 1;
    [Header("Shop")]
    public Transform shopCardSpawn;
    public Transform shopAugmentSpawn;
    public GameobjectLootpool cardProbs;
    public Transform textParent;
    [Header("Battle")]
    public static GameObject inventoryLabels;
    public static GameObject inventoryUI;
    public static ScrollArea inventoryLootScrollArea;
    public static GameObject boonCurse;
    public static GameObject riskRewardTextbox;
    [Header("Upgrade")]
    public int test2 = 10;
    [Header("Augment")]
    public static ScrollArea augCardScrollArea;
    public static ScrollArea augAugmentScrollArea;
    public GameObject cardSAI;

    private void Awake()
    {
        main = this;
        inventoryLabels = GameObject.Find("Inventory Labels");
        inventoryUI = GameObject.Find("Inventory UI");
        boonCurse = GameObject.Find("Boon-curse");
        darkenOverlay = GameObject.Find("Screen Darken");
        riskRewardTextbox = GameObject.Find("Risk-Reward Textbox");
        augCardScrollArea = GameObject.Find("Augment Deck Scroll Area").GetComponent<ScrollArea>();
        augAugmentScrollArea = GameObject.Find("Augment Scroll Area").GetComponent<ScrollArea>();
        inventoryLootScrollArea = inventoryUI.GetComponentInChildren<ScrollArea>();
    }

    private void Start()
    {
        BGMat = GameObject.Find("Background Shader").GetComponent<SpriteRenderer>().material;
        inventoryLabels.SetActive(false);
        inventoryUI.SetActive(false);
        boonCurse.SetActive(false);
        darkenOverlay.SetActive(false);
        riskRewardTextbox.SetActive(false);
        RoundBGMat();
    }
    private void Update()
    {
        // move camera
        if (Vector3.Distance(Camera.main.transform.position, cameraDestination) > 0.02f)
            Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, cameraDestination, Time.deltaTime * 5);
        else
            Camera.main.transform.position = cameraDestination;

        // lerp background
        if (lerpProgress < 0.995)
        {
            LerpBGMat();
        }
        else if (lerpProgress != 1)
        {
            RoundBGMat();
        }

        // lerp timescale
        if (Mathf.Abs(timeScale - Time.timeScale) > 0.1f)
        {
            Time.timeScale = Mathf.Lerp(Time.timeScale, timeScale, 5 * Time.deltaTime);
        }
        else
        {
            Time.timeScale = timeScale;
        }
    }

    public static void SwitchStage(Stage stage)
    {
        lerpProgress = 0;

        // FROM
        switch (currentStage)
        {
            case Stage.Map:
                break;
            case Stage.Battle:
                inventoryUI.SetActive(false);
                inventoryLabels.SetActive(false);
                inventoryLootScrollArea.ClearClaimed();
                boonCurse.SetActive(false);
                Hand.Clear();
                BattleButton.main.spriteUp = BattleButton.main.playUp;
                BattleButton.main.spriteDown = BattleButton.main.playDown;
                Spawner.main.stats.ModifyStat("hp_mult", 1.2f, Stats.Operation.Multiply); // Every round is 20% healthier than previous
                Spawner.main.stats.ModifyStat("speed", 0.05f);
                ToggleDarken(false);
                ToggleTime(true);
                break;
            case Stage.Shop:
                ShopController.ResetShop();
                break;
            case Stage.Augment:
                augCardScrollArea.DeleteInventory();
                augAugmentScrollArea.DeleteInventory();
                AugmentTable.main.PurgeStowaways();
                break;
            case Stage.Upgrade:
                ShopController.shopScrollArea.DeleteInventory();
                UpgradeTable.main.PurgeStowaways();
                break;
            default:
                break;
        }

        // TO
        switch (stage)
        {
            case Stage.Map:
                cameraDestination = new Vector3(0, -10, -10);
                currentStage = Stage.Map;
                break;
            case Stage.Battle:
                cameraDestination = new Vector3(0, 0, -10);
                currentStage = Stage.Battle;
                main.SetupBattle();
                break;
            case Stage.Shop:
                cameraDestination = new Vector3(0, -20, -10);
                currentStage = Stage.Shop;
                main.SetupShop();
                break;
            case Stage.Augment:
                cameraDestination = new Vector3(-25, -10, -10);
                currentStage = Stage.Augment;
                main.SetupAugment();
                break;
            case Stage.Upgrade:
                cameraDestination = new Vector3(25, -10, -10);
                currentStage = Stage.Upgrade;
                main.SetupUpgrade();
                break;
            default:
                break;
        }
    }

    public void SetupBattle()
    {
        Hand.Deal();
        Hand.Display(false);
        Spawner.main.complete = false;
        BattleButton.main.SetActive(true);
    }


    public void SetupShop()
    {
        ScrollArea cardScrollArea = GameObject.Find("Shop Deck Scroll Area").GetComponent<ScrollArea>();
        cardScrollArea.FillWithCards(cardSAI, transform, 0, Cards.CardType.Card);

        for (int i = 0; i<ShopController.main.cardCount; i++) // cards
        {
            Vector3 pos = shopCardSpawn.position + i * Vector3.right - ShopController.main.cardCount / 2f * Vector3.right;
            GameObject cardObj = ShopController.MakeCard(pos);
            ShopItem cardItem = cardObj.GetComponent<ShopItem>();
            cardObj.transform.parent = shopCardSpawn;
            GameObject label = ShopController.MakeLabel(pos + Vector3.down, cardItem.GetPrice() + "c");
            if (cardItem.GetDiscount() != 1)
            {
                label.GetComponent<TMPLabel>().SetText(label.GetComponent<TMPLabel>().GetText());
            }
            label.transform.SetParent(textParent, true);
        }
        for (int i = 0; i < ShopController.main.augmentCount; i++) // augments
        {
            Vector3 pos = shopAugmentSpawn.position + i * Vector3.right - ShopController.main.augmentCount / 2f * Vector3.right;
            GameObject augmentObj = ShopController.MakeAugment(pos);
            ShopItem augmentItem = augmentObj.GetComponent<ShopItem>();
            augmentObj.transform.parent = shopAugmentSpawn;
            GameObject label = ShopController.MakeLabel(pos + Vector3.down, augmentItem.GetPrice() + "c");
            if (augmentItem.GetDiscount() != 1)
            {
                label.GetComponent<TMPLabel>().SetText(label.GetComponent<TMPLabel>().GetText());
            }
            label.transform.SetParent(textParent, true);
        }
    }

    public void SetupUpgrade()
    {
        UpgradeTable.upgrades = 0; // resets upgrade cost scaling

        ScrollArea cardScrollArea = GameObject.Find("Upgrade Deck Scroll Area").GetComponent<ScrollArea>();
        Transform cardDestination = UpgradeTable.main.transform.GetChild(0);
        cardScrollArea.FillWithCards(cardSAI, cardDestination, 0, Cards.CardType.Card);
    }

    public void SetupAugment()
    {
        Transform cardDestination = AugmentTable.main.transform.GetChild(1);
        Transform augmentDestination = AugmentTable.main.transform.GetChild(0);

        augCardScrollArea.FillWithCards(cardSAI, cardDestination, 0, Cards.CardType.Card);
        augAugmentScrollArea.FillWithCards(cardSAI, augmentDestination, 1, Cards.CardType.Augment);
    }

    public static void ToggleDarken(bool active)
    {
        darkenOverlay.SetActive(active);
    }

    public static void ToggleTime(bool active)
    {
        ToggleTime(active, 0.1f);
    }

    public static void ToggleTime(bool active, float tScale)
    {
        timeScale = active ? (BattleButton.phase == 1 ? BattleButton.speed : 1) : tScale;
    }

    private static void LerpBGMat()
    {
        int stageIndex = (int)currentStage-1;
        float speed = 5;
        lerpProgress = Mathf.Lerp(lerpProgress, 1, Time.deltaTime * speed);

        Color lerpedOverlapColor = Color.Lerp(BGMat.GetColor("_Overlap_Color"), main.stageMaterials[stageIndex].overlapColor, Time.deltaTime * speed);
        Color lerpedHighColor = Color.Lerp(BGMat.GetColor("_High_Color"), main.stageMaterials[stageIndex].highColor, Time.deltaTime * speed);
        Color lerpedLowColor = Color.Lerp(BGMat.GetColor("_Low_Color"), main.stageMaterials[stageIndex].lowColor, Time.deltaTime * speed);
        float lerpedBlobStep = Mathf.Lerp(BGMat.GetFloat("_Blob_Step"), main.stageMaterials[stageIndex].blobStep, Time.deltaTime * speed);
        float lerpedBlobPower = Mathf.Lerp(BGMat.GetFloat("_Blob_Power"), main.stageMaterials[stageIndex].blobPower, Time.deltaTime * speed);
        float lerpedBlobDensity = Mathf.Lerp(BGMat.GetFloat("_Blob_Density"), main.stageMaterials[stageIndex].blobDensity, Time.deltaTime * speed);
        float lerpedShearStrength = Mathf.Lerp(BGMat.GetFloat("_Shear_Strength"), main.stageMaterials[stageIndex].shearStrength, Time.deltaTime * speed);
        float lerpedGradientNoiseStep = Mathf.Lerp(BGMat.GetFloat("_Gradient_Noise_Step"), main.stageMaterials[stageIndex].gradientNoiseStep, Time.deltaTime * speed);

        BGMat.SetColor("_Overlap_Color", lerpedOverlapColor);
        BGMat.SetColor("_High_Color", lerpedHighColor);
        BGMat.SetColor("_Low_Color", lerpedLowColor);
        BGMat.SetFloat("_Blob_Step", lerpedBlobStep);
        BGMat.SetFloat("_Blob_Power", lerpedBlobPower);
        BGMat.SetFloat("_Blob_Density", lerpedBlobDensity);
        BGMat.SetFloat("_Shear_Strength", lerpedShearStrength);
        BGMat.SetFloat("_Gradient_Noise_Step", lerpedGradientNoiseStep);
    }

    private static void RoundBGMat()
    {
        lerpProgress = 1;
        int stageIndex = (int)currentStage-1;

        BGMat.SetColor("_Overlap_Color", main.stageMaterials[stageIndex].overlapColor);
        BGMat.SetColor("_High_Color", main.stageMaterials[stageIndex].highColor);
        BGMat.SetColor("_Low_Color", main.stageMaterials[stageIndex].lowColor);
        BGMat.SetFloat("_Blob_Step", main.stageMaterials[stageIndex].blobStep);
        BGMat.SetFloat("_Blob_Power", main.stageMaterials[stageIndex].blobPower);
        BGMat.SetFloat("_Blob_Density", main.stageMaterials[stageIndex].blobDensity);
        BGMat.SetFloat("_Shear_Strength", main.stageMaterials[stageIndex].shearStrength);
        BGMat.SetFloat("_Gradient_Noise_Step", main.stageMaterials[stageIndex].gradientNoiseStep);
    }
}
