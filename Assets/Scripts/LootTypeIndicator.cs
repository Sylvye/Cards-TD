using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class LootTypeIndicator : MonoBehaviour
{
    public int category;
    public SpriteHolder sprites;
    public static bool firstDrop = true;
    private float spawnTime;
    
    private SpriteRenderer sr;
    private Vector3 destination;


    private void Start()
    {
        spawnTime = Time.time;
        sr = GetComponent<SpriteRenderer>();
        if (firstDrop)
        {
            category = 0;
            firstDrop = false;
        }
        else
        {
            category = Random.Range(0, 2);
        }
        Main.main.packs[category]++;
        sr.sprite = sprites.sprites[category];
    }

    private void Update()
    {
        if (Time.time > spawnTime+0.75f)
        {
            if (destination == null)
                destination = transform.position + Vector3.up * 12;
            if (Vector2.Distance(transform.position, destination) > 0.1f)
            {
                transform.position = Vector2.Lerp(transform.position, destination, Time.deltaTime * 10);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
