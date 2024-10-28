using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropIndicator : MonoBehaviour
{
    public string category = "";
    public SpriteHolder sprites;
    public static string[] categories = { "Fighter", "Hoarder", "Artisan" };
    public static bool firstDropArtisan = false;
    private SpriteRenderer sr;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        if (!firstDropArtisan)
        {
            category = categories[2];
            firstDropArtisan = true;
        }
        category = categories[Random.Range(0, 3)];
        sr.sprite = sprites.sprites[CategoryToNum(category)];
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
}
