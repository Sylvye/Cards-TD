using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class ItemDrop : MonoBehaviour
{
    public static string[] categories = { "Fighter", "Hoarder", "Artisan" };
    public static bool firstDropArtisan = false;
    public string itemName;
    public string category;
    public GameObject indicator;
    public int tier;
    public float fadeAmt = 1;
    SpriteRenderer sr;
    Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        float layer = tier;
        transform.position -= Vector3.forward * layer * 0.5f;
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        if (!firstDropArtisan)
        {
            category = categories[2];
            firstDropArtisan = true;
        }
        category = categories[Random.Range(0, 3)];
    }

    private void OnMouseOver()
    {
        GameObject ind = Instantiate(indicator, transform.position, Quaternion.identity);
        indicator.GetComponent<Rigidbody2D>().AddForce(Vector2.up*20);
        Destroy(gameObject);
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
