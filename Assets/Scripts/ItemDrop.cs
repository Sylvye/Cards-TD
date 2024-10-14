using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class ItemDrop : MonoBehaviour
{
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
    }

    // Update is called once per frame
    void Update()
    {
        Color c = sr.color;
        if (c.a - fadeAmt < 0 )
        {
            sr.color = new Color(c.r, c.g, c.b, c.a - fadeAmt);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnMouseOver()
    {
        rb.AddForce(Vector2.up * 2);
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
