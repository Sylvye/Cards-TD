using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Unity.VisualScripting;
using UnityEngine;

public class ShopController : MonoBehaviour
{
    public static ShopController main;
    public GameObject cardPrefab;
    public GameObject augmentPrefab;
    public GameObject labelPrefab;
    public int cardCount;
    public int augmentCount;
    public GameobjectLootpool cardLootpool;
    public GameobjectLootpool commonAugmentLootpool;
    public GameobjectLootpool rareAugmentLootpool;
    public GameobjectLootpool uniqueAugmentLootpool;
    public AnimationCurve tierChances;
    public static ScrollAreaInventory shopScrollArea;

    private static List<GameObject> shopStuff = new();

    // Start is called before the first frame update
    void Start()
    {
        main = this;
        shopScrollArea = GameObject.Find("Shop Deck Scroll Area").GetComponent<ScrollAreaInventory>();
    }

    public static GameObject MakeCard()
    {
        return MakeCard(new Vector3(0, 10, 0));
    }

    public static GameObject MakeCard(Vector3 spawn)
    {
        CardPurchaseable output = Instantiate(main.cardPrefab, spawn, Quaternion.identity).GetComponent<CardPurchaseable>();

        output.card = main.cardLootpool.GetRandom().GetComponent<Card>();
        output.tier = Random.Range(1, Mathf.RoundToInt(main.tierChances.Evaluate(MapController.GetProgress()) * 5 + 1));
        shopStuff.Add(output.gameObject);
        return output.gameObject;
    }

    public static GameObject MakeAugment()
    {
        return MakeAugment(new Vector3(0, 10, 0));
    }

    public static GameObject MakeAugment(Vector3 spawn)
    {
        AugmentPurchaseable output = Instantiate(main.augmentPrefab, spawn, Quaternion.identity).GetComponent<AugmentPurchaseable>();

        int r = Random.Range(0, 100);
        if (r < 50)
        {
            output.augment = main.commonAugmentLootpool.GetRandom().GetComponent<Augment>();
        } else if (r < 80)
        {
            output.augment = main.rareAugmentLootpool.GetRandom().GetComponent<Augment>();
        } else
        {
            output.augment = main.uniqueAugmentLootpool.GetRandom().GetComponent<Augment>();
        }

        shopStuff.Add(output.gameObject);
        return output.gameObject;
    }

    public static GameObject MakeLabel(Vector3 spawn, string text)
    {
        GameObject output = Instantiate(main.labelPrefab, spawn, Quaternion.identity);

        output.GetComponent<TMPLabel>().SetText(text);
        shopStuff.Add(output);
        return output;
    }

    public static void ResetShop()
    {
        shopScrollArea.DeleteInventory();

        foreach (GameObject obj in shopStuff)
        {
            Destroy(obj);
        }
    }
}
