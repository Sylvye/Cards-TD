using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using UnityEngine.XR;

public class StageController : MonoBehaviour
{
    public static int stageIndex = 0;
    private static Vector3 cameraDestination = new(0, -10, -10);
    [Header("Shop")]
    public GameobjectLootpool cardProbs;
    public GameObject cardOption;
    public float[] rarityWeights = { 78, 12, 6, 3, 1 };
    public static float[] rarityWeights_;
    private static GameobjectLootpool cardProbs_;
    private static GameObject cardOption_;
    private static GameObject[] shopCards = new GameObject[3];
    public static GameObject battleButton;
    public static GameObject inventoryOverlay;
    public static GameObject inventoryUI;
    public static ScrollArea inventoryLootScrollArea;

    [Header("Upgrade")]
    public int test2 = 10;
    [Header("Augment")]
    public GameObject cardItem;
    public static GameObject cardItem_;

    private void Start()
    {
        rarityWeights_ = new float[3];
        cardProbs_ = cardProbs;
        cardOption_ = cardOption;
        cardItem_ = cardItem;
        battleButton = GameObject.Find("Battle Button");
        inventoryOverlay = GameObject.Find("Inventory Overlay");
        inventoryOverlay.SetActive(false);
        inventoryUI = GameObject.Find("Inventory UI");
        inventoryUI.SetActive(false);
        inventoryLootScrollArea = inventoryOverlay.GetComponentInChildren<ScrollArea>();
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
        foreach (GameObject obj in shopCards) // clears card options on scene change
        {
            if (obj != null)
            {
                Destroy(obj);
                Destroy(obj.GetComponent<ShopCard>().card.gameObject);
            }
        }

        switch (name)
        {
            case "Map":
                cameraDestination = new Vector3(0, -10, -10);
                stageIndex = 0;
                break;
            case "Defense":
                cameraDestination = new Vector3(0, 0, -10);
                stageIndex = 1;
                SetupBattle();
                break;
            case "Shop":
                cameraDestination = new Vector3(0, -20, -10);
                stageIndex = 2;
                SetupShop();
                break;
            case "Augment":
                cameraDestination = new Vector3(-25, -10, -10);
                stageIndex = 3;
                SetupAugment();
                break;
            case "Upgrade":
                cameraDestination = new Vector3(25, -10, -10);
                stageIndex = 4;
                break;
            default:
                break;
        }
    }

    public static void SetupBattle()
    {
        Hand.Deal();
        Spawner.main.complete = false;
        battleButton.GetComponent<BattleButton>().SetActive(true);
    }


    public static void SetupShop()
    {
        for (int i = 0; i < 3; i++)
        {
            GameObject cardOptionObj = Instantiate(cardOption_, new Vector2(-5 + i * 5, -20), Quaternion.identity);
            ShopCard co = cardOptionObj.GetComponent<ShopCard>();
            co.card = Instantiate(cardProbs_.GetRandom().GetComponent<Card>(), new Vector2(0, 10), Quaternion.identity);
            co.card.tier = WeightedRandom.SelectWeightedIndex(new List<float>(rarityWeights_)) + 1;
            cardOptionObj.GetComponent<SpriteRenderer>().sprite = Resources.LoadAll<Sprite>("CardPack")[co.card.towerIndex * 5 + co.card.tier - 1];
            shopCards[i] = cardOptionObj;
        }
    }

    public static void SetupUpgrade()
    {

    }

    public static void SetupAugment()
    {
        ScrollArea cardScrollArea = GameObject.Find("Augment Deck Scroll Area").GetComponent<ScrollArea>();
        ScrollArea augmentScrollArea = GameObject.Find("Augment Scroll Area").GetComponent<ScrollArea>();
        GameObject cardDestination = GameObject.Find("Card Slot");
        GameObject augmentDestination = GameObject.Find("Augment Slot");
        for (int i=0; i<Cards.DeckSize(); i++) // places cards in deck scroll area
        {
            GameObject itemObj = Instantiate(cardItem_, Vector3.one, Quaternion.identity);
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
            GameObject itemObj = Instantiate(cardItem_, Vector3.one, Quaternion.identity);
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
