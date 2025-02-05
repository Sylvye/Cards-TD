using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

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
    private static Vector3 cameraDestination = new(0, -10, -10);
    [Header("Shop")]
    public Transform shopCardSpawn;
    public Transform shopAugmentSpawn;
    public GameobjectLootpool cardProbs;
    public Transform textParent;
    public float[] rarityWeights = { 78, 12, 6, 3, 1 };
    [Header("Battle")]
    public static GameObject battleButton;
    public static GameObject inventoryLabels;
    public static GameObject inventoryUI;
    public static ScrollArea inventoryLootScrollArea;
    public static GameObject boonCurse;
    public static GameObject riskRewardTextbox;
    [Header("Upgrade")]
    public int test2 = 10;
    [Header("Augment")]
    public GameObject cardSAI;
    public GameObject augmentSAI;

    private void Awake()
    {
        main = this;
        battleButton = GameObject.Find("Battle Button");
        inventoryLabels = GameObject.Find("Inventory Labels");
        inventoryUI = GameObject.Find("Inventory UI");
        boonCurse = GameObject.Find("Boon-curse");
        riskRewardTextbox = GameObject.Find("Risk-Reward Textbox");
        inventoryLootScrollArea = inventoryUI.GetComponentInChildren<ScrollArea>();
    }

    private void Start()
    {
        inventoryLabels.SetActive(false);
        inventoryUI.SetActive(false);
        boonCurse.SetActive(false);
        riskRewardTextbox.SetActive(false);
    }
    private void Update()
    {
        if (Vector3.Distance(Camera.main.transform.position, cameraDestination) > 0.02f)
            Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, cameraDestination, Time.deltaTime * 5);
        else
            Camera.main.transform.position = cameraDestination;
    }

    public static void SwitchStage(Stage stage)
    {
        if (currentStage == Stage.Shop)
        {
            Shop.ResetShop();
        }

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
        Spawner.main.complete = false;
        battleButton.GetComponent<BattleButton>().SetActive(true);
    }


    public void SetupShop()
    {
        ScrollArea cardScrollArea = GameObject.Find("Shop Deck Scroll Area").GetComponent<ScrollArea>();
        cardScrollArea.FillWithCards(cardSAI, transform, 0, Cards.CardType.Card);

        for (int i = 0; i<Shop.main.cardCount; i++) // cards
        {
            float scale = 2;
            Vector3 pos = shopCardSpawn.position + i * scale * Vector3.right - Shop.main.cardCount * scale / 2f * Vector3.right;
            GameObject cardObj = Shop.MakeCard(pos);
            ShopItem cardItem = cardObj.GetComponent<ShopItem>();
            cardObj.transform.parent = shopCardSpawn;
            GameObject label = Shop.MakeLabel(pos + Vector3.down, cardItem.GetPrice() + "c");
            label.transform.SetParent(textParent, true);
        }
        for (int i = 0; i < Shop.main.augmentCount; i++) // augments
        {
            float scale = 2;
            Vector3 pos = shopAugmentSpawn.position + i * scale * Vector3.right - Shop.main.augmentCount * scale / 2f * Vector3.right;
            GameObject augmentObj = Shop.MakeAugment(pos);
            ShopItem augmentItem = augmentObj.GetComponent<ShopItem>();
            augmentObj.transform.parent = shopAugmentSpawn;
            GameObject label = Shop.MakeLabel(pos + Vector3.down, augmentItem.GetPrice() + "c");
            label.transform.SetParent(textParent, true);
        }
    }

    public void SetupUpgrade()
    {
        UpgradeTable.upgrades = 0;

        ScrollArea cardScrollArea = GameObject.Find("Upgrade Deck Scroll Area").GetComponent<ScrollArea>();
        Transform cardDestination = UpgradeTable.main.transform.GetChild(0);
        cardScrollArea.FillWithCards(cardSAI, cardDestination, 0, Cards.CardType.Card);
    }

    public void SetupAugment()
    {
        ScrollArea cardScrollArea = GameObject.Find("Augment Deck Scroll Area").GetComponent<ScrollArea>();
        ScrollArea augmentScrollArea = GameObject.Find("Augment Scroll Area").GetComponent<ScrollArea>();
        Transform cardDestination = AugmentTable.main.transform.GetChild(1);
        Transform augmentDestination = AugmentTable.main.transform.GetChild(0);

        cardScrollArea.FillWithCards(cardSAI, cardDestination, 0, Cards.CardType.Card);
        augmentScrollArea.FillWithCards(augmentSAI, augmentDestination, 1, Cards.CardType.Augment);
    }
}
