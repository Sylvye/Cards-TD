using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class StageController : MonoBehaviour
{
    public static int stageIndex = 0;
    private static Vector3 destination = new(0, -8, -10);
    [Header("Shop")]
    public CardProbs cardProbs;
    public GameObject cardOption;
    [Header("Upgrade")]
    public int test2 = 10;
    [Header("Augment")]
    public int test3 = 10;

    private static CardProbs cardProbs_;
    private static GameObject cardOption_;

    private void Start()
    {
        cardProbs_ = cardProbs;
        cardOption_ = cardOption;
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
        switch (name)
        {
            case "Map":
                destination = new Vector3(0, -8, -10);
                stageIndex = 0;
                break;
            case "Defense":
                destination = new Vector3(0, 0, -10);
                stageIndex = 1;
                Spawner.main.complete = false;
                break;
            case "Shop":
                destination = new Vector3(0, -8, -10);
                stageIndex = 2;
                SetupShop();
                break;
            case "Augment":
                destination = new Vector3(-25, -8, -10);
                stageIndex = 3;
                break;
            case "Upgrade":
                destination = new Vector3(25, -8, -10);
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
            GameObject Card = Instantiate(cardOption_, new Vector2(-5 + i * 5, -9), Quaternion.identity);
            CardOption cardOption = Card.GetComponent<CardOption>();
            cardOption.card = cardProbs_.GetRandom();
            Card.GetComponent<SpriteRenderer>().sprite = cardOption.card.gameObject.GetComponent<SpriteRenderer>().sprite;
        }
    }

    public static void SetupUpgrade()
    {

    }

    public static void SetupAugment()
    {

    }
}
