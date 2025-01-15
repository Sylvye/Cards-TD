using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class StageController : MonoBehaviour
{
    [Header("General")]
    public static StageController main;
    public static int stageIndex = 0;
    private static Vector3 cameraDestination = new(0, -10, -10);
    [Header("Shop")]
    public Transform shopCardSpawn;
    public Transform shopAugmentSpawn;
    public GameobjectLootpool cardProbs;
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

    public static void SwitchStage(string name)
    {
        switch (name)
        {
            case "Map":
                cameraDestination = new Vector3(0, -10, -10);
                stageIndex = 0;
                break;
            case "Defense":
                cameraDestination = new Vector3(0, 0, -10);
                stageIndex = 1;
                main.SetupBattle();
                break;
            case "Shop":
                cameraDestination = new Vector3(0, -20, -10);
                stageIndex = 2;
                main.SetupShop();
                break;
            case "Augment":
                cameraDestination = new Vector3(-25, -10, -10);
                stageIndex = 3;
                main.SetupAugment();
                break;
            case "Upgrade":
                cameraDestination = new Vector3(25, -10, -10);
                stageIndex = 4;
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
        for (int i = 0; i<Shop.main.cardCount; i++)
        {
            float scale = 2;
            GameObject card = Shop.MakeCard(shopCardSpawn.position + i * scale * Vector3.right - Shop.main.cardCount * scale / 2f * Vector3.right);
            card.transform.parent = shopCardSpawn;
        }
        for (int i = 0; i < Shop.main.augmentCount; i++)
        {
            float scale = 2;
            GameObject augment = Shop.MakeAugment(shopAugmentSpawn.position + i * scale * Vector3.right - Shop.main.augmentCount * scale / 2f * Vector3.right);
            augment.transform.parent = shopAugmentSpawn;
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
