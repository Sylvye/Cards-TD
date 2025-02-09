using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class LootTypeIndicator : MonoBehaviour
{
    public int category;
    public SpriteHolder sprites;
    public static bool firstDrop = true;
    
    private SpriteRenderer sr;
    private Transform destination;


    private void Start()
    {
        destination = GameObject.Find("Pack Icon").transform;
        sr = GetComponent<SpriteRenderer>();
        if (firstDrop)
        {
            category = 0;
            firstDrop = false;
        }
        else
        {
            category = Random.Range(0, 3);
        }
        sr.sprite = sprites.sprites[category];
    }

    private void Update()
    {
        if (Vector2.Distance(transform.position, destination.position) > 0.1f)
        {
            transform.position = Vector2.Lerp(transform.position, destination.position, Time.deltaTime * 10);
        }
        else
        {
            Main.main.packs[category]++;
            Destroy(gameObject);
        }
    }
}
