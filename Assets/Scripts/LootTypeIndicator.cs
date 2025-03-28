using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class LootTypeIndicator : MonoBehaviour
{
    public int category;
    public SpriteHolder sprites;
    public Vector2 destination;
    public static bool firstDrop = true;
    private float spawnTime;
    private SpriteRenderer sr;


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
            // 15% chance for an augment
            int rand = Random.Range(0, 100);
            category = rand < 15 ? 0 : 1; // 1 = drop, 0 = augment
        }
        Main.main.packs[category]++;
        sr.sprite = sprites.sprites[category];
    }

    private void Update()
    {
        if (Time.time > spawnTime+0.75f)
        {
            if (Vector2.Distance(transform.position, destination) > 0.1f)
            {
                transform.position = (Vector3)Vector2.Lerp(transform.position, destination, Time.unscaledDeltaTime * 2) + Vector3.back * 1;
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
