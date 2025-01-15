using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public class Shop : MonoBehaviour
{
    public static Shop main;
    public GameObject cardPrefab;
    public GameObject augmentPrefab;
    public int cardCount;
    public int augmentCount;
    public GameobjectLootpool cardLootpool;
    public GameobjectLootpool augmentLootpool;

    // Start is called before the first frame update
    void Start()
    {
        main = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static GameObject MakeCard()
    {
        CardPurchaseable output = Instantiate(main.cardPrefab, new Vector3(0, 10, 0), Quaternion.identity).GetComponent<CardPurchaseable>();

        output.card = main.cardLootpool.GetRandom().GetComponent<Card>();
        return output.gameObject;
    }

    public static GameObject MakeCard(Vector3 spawn)
    {
        CardPurchaseable output = Instantiate(main.cardPrefab, spawn, Quaternion.identity).GetComponent<CardPurchaseable>();

        output.card = main.cardLootpool.GetRandom().GetComponent<Card>();
        return output.gameObject;
    }

    public static GameObject MakeAugment()
    {
        AugmentPurchaseable output = Instantiate(main.augmentPrefab, new Vector3(0, 10, 0), Quaternion.identity).GetComponent<AugmentPurchaseable>();

        output.augment = main.augmentLootpool.GetRandom().GetComponent<Augment>();
        return output.gameObject;
    }

    public static GameObject MakeAugment(Vector3 spawn)
    {
        AugmentPurchaseable output = Instantiate(main.augmentPrefab, spawn, Quaternion.identity).GetComponent<AugmentPurchaseable>();

        output.augment = main.augmentLootpool.GetRandom().GetComponent<Augment>();
        return output.gameObject;
    }
}
