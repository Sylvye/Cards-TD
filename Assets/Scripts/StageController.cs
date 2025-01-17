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
    public static GameObject inventoryOverlay;
    public static GameObject inventoryUI;
    public static ScrollArea inventoryLootScrollArea;
    [Header("Upgrade")]
    public int test2 = 10;
    [Header("Augment")]
    public GameObject cardItem;

    private void Start()
    {
        battleButton = GameObject.Find("Battle Button");
        inventoryOverlay = GameObject.Find("Inventory Overlay");
        inventoryOverlay.SetActive(false);
        inventoryUI = GameObject.Find("Inventory UI");
        inventoryUI.SetActive(false);
        inventoryLootScrollArea = inventoryOverlay.GetComponentInChildren<ScrollArea>();
        main = this;
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
            Shop.ClearShop();
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

    }

    public void SetupAugment()
    {
        ScrollArea cardScrollArea = GameObject.Find("Augment Deck Scroll Area").GetComponent<ScrollArea>();
        ScrollArea augmentScrollArea = GameObject.Find("Augment Scroll Area").GetComponent<ScrollArea>();
        GameObject cardDestination = GameObject.Find("Card Slot");
        GameObject augmentDestination = GameObject.Find("Augment Slot");
        for (int i=0; i<Cards.DeckSize(); i++) // places cards in deck scroll area
        {
            GameObject itemObj = Instantiate(cardItem, Vector3.one, Quaternion.identity);
            SpriteRenderer sr = itemObj.GetComponent<SpriteRenderer>();
            AugmentSceneScrollAreaItem item = itemObj.GetComponent<AugmentSceneScrollAreaItem>();
            Card c = Cards.GetFromDeck(i);
            sr.sortingOrder = 0;
            sr.sprite = c.GetComponent<SpriteRenderer>().sprite;
            cardScrollArea.AddToInventory(itemObj);
            item.draggableDestinations.Add(cardDestination);
            item.reference = c.gameObject;
            item.id = item.reference.GetComponent<TowerCard>().GetName();
        }
        for (int i = 0; i < Cards.AugmentSize(); i++) // places augments in augment scroll area
        {
            GameObject itemObj = Instantiate(cardItem, Vector3.one, Quaternion.identity);
            SpriteRenderer sr = itemObj.GetComponent<SpriteRenderer>();
            AugmentSceneScrollAreaItem item = itemObj.GetComponent<AugmentSceneScrollAreaItem>();
            Augment a = Cards.GetFromAugments(i);
            sr.sortingOrder = 1;
            sr.sprite = a.GetComponent<SpriteRenderer>().sprite;
            augmentScrollArea.AddToInventory(itemObj);
            item.draggableDestinations.Add(augmentDestination);
            item.reference = a.gameObject;
            item.id = item.reference.GetComponent<Augment>().type;
        }
    }
}
