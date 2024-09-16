using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;

public class StageController : MonoBehaviour
{
    public static int stageIndex = 0;
    private static Vector3 destination = new(0, -10, -10);
    [Header("Shop")]
    public Probabilities cardProbs;
    public GameObject cardOption;
    public float[] rarityWeights = { 78, 12, 6, 3, 1 };
    public static float[] rarityWeights_;
    private static Probabilities cardProbs_;
    private static GameObject cardOption_;
    private static GameObject[] shopCards = new GameObject[3];
    [Header("Upgrade")]
    public int test2 = 10;
    [Header("Augment")]
    public GameObject item;
    public static GameObject item_;

    private void Start()
    {
        rarityWeights_ = new float[3];
        cardProbs_ = cardProbs;
        cardOption_ = cardOption;
        item_ = item;
    }
    private void Update()
    {
        if (Vector3.Distance(Camera.main.transform.position, destination) > 0.02f)
            Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, destination, Time.deltaTime * 5);
        else
            Camera.main.transform.position = destination;
    }

    public static void SwitchStage(string name)
    {
        foreach (GameObject obj in shopCards) // clears card options on scene change
        {
            if (obj != null)
            {
                Destroy(obj);
                Destroy(obj.GetComponent<CardOption>().card.gameObject);
            }
        }

        switch (name)
        {
            case "Map":
                destination = new Vector3(0, -10, -10);
                stageIndex = 0;
                break;
            case "Defense":
                Hand.main.Deal();
                Hand.main.DisplayCards();
                destination = new Vector3(0, 0, -10);
                stageIndex = 1;
                Spawner.main.complete = false;
                break;
            case "Shop":
                destination = new Vector3(0, -20, -10);
                stageIndex = 2;
                SetupShop();
                break;
            case "Augment":
                destination = new Vector3(-25, -10, -10);
                stageIndex = 3;
                SetupAugment();
                break;
            case "Upgrade":
                destination = new Vector3(25, -10, -10);
                stageIndex = 4;
                break;
            default:
                break;
        }
    }
    public static void SetupShop()
    {
        for (int i = 0; i < 3; i++)
        {
            GameObject cardOptionObj = Instantiate(cardOption_, new Vector2(-5 + i * 5, -20), Quaternion.identity);
            CardOption co = cardOptionObj.GetComponent<CardOption>();
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
        int vertOffset = 0;
        for (int i=0; i<Cards.deck.Count; i++)
        {
            if (i % 5 == 0)
            {
                vertOffset += 40;
            }
            GameObject itemObj = Instantiate(item_, Vector3.zero, Quaternion.identity);
            itemObj.GetComponent<Image>().sprite = Cards.deck[i].GetComponent<SpriteRenderer>().sprite;
            itemObj.transform.localPosition = new Vector2(i%5 * 50 + 50, 0 - vertOffset);
        }
    }
}
