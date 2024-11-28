using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class DropIndicator : MonoBehaviour
{
    public string category = "";
    public SpriteHolder sprites;
    public GameobjectLootpool cardLootpool;
    public GameobjectLootpool augmentLootpool;
    public int currencyMax = 5;
    public int currencyMin = 25;
    public static string[] categories = { "Fighter", "Hoarder", "Artisan" };
    public static bool firstDropArtisan = false;
    
    private SpriteRenderer sr;
    private Transform destination;


    private void Start()
    {
        destination = GameObject.Find("Pack Icon").transform;
        sr = GetComponent<SpriteRenderer>();
        if (!firstDropArtisan)
        {
            category = categories[2];
            firstDropArtisan = true;
        }
        category = categories[Random.Range(0, 3)];
        sr.sprite = sprites.sprites[CategoryToNum(category)];
    }

    private void Update()
    {
        if (Vector2.Distance(transform.position, destination.position) > 0.1f)
        {
            transform.position = Vector2.Lerp(transform.position, destination.position, Time.deltaTime * 10);
        }
        else
        {
            Main.main.packs[CategoryToNum(category)]++;
            Destroy(gameObject);
        }
    }

    public int CategoryToNum(string r)
    {
        return r switch
        {
            "Fighter" => 0,
            "Hoarder" => 1,
            "Artisan" => 2,
            _ => -1,
        };
    }

    /*private void DrawLoot(int pulls)
    {
        switch (category)
        {
            case "Fighter":
                for (int i=0; i<pulls; i++)
                {
                    // add cards, BUT - what if a player doesnt want the card? TBD
                }
                return;
            case "Hoarder":
                return;
            case "Artisan":
                for (int i=0; i<pulls; i++)
                {
                    // grants a random augment
                    Cards.augments.Add(augmentLootpool.objects[WeightedRandom.SelectWeightedIndex(augmentLootpool.weights)].GetComponent<Augment>());
                }
                return;
        }
    }*/
}
