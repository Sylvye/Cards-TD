using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Build;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Spawner : MonoBehaviour
{
    public static Spawner main;
    public static List<GameObject> spawnedEnemies = new List<GameObject>();
    public List<GameObject> enemies;
    public List<GameObject> wave;
    public float density = 1;
    public bool active = true;
    public bool complete = false;
    private int spawned; // how many enemies spawned this wave
    private float cooldown = 0;
    [SerializeField]
    private bool freebie = false;

    private void Start()
    {
        main = this;
    }

    // Update is called once per frame
    private void Update()
    {
        if (active)
        {
            if (wave.Count > 0)
            {
                cooldown -= Time.deltaTime;

                if (cooldown <= 0)
                {
                    if (wave[0] != null)
                    {
                        cooldown = 1 / density;
                        Spawn(wave[0], transform.position, wave[0].transform.rotation);
                        wave.RemoveAt(0);
                    }
                    else
                    {
                        cooldown = 1 / density; // later, will make a "wait" class that stores a value that is added to the cooldown.
                    }
                }
            }
            else
            {
                Complete();
            }
        }
    }

    private void Complete()
    {
        active = false;
        complete = true;
        freebie = false;
        cooldown = 0;
        spawned = 0;
    }

    public bool IsStageComplete()
    {
        return StageController.stageIndex == 1 && complete && spawnedEnemies.Count == 0;
    }

    public void Send(int tier)
    {
        Spawn(enemies[tier - 1], transform.position, enemies[tier - 1].transform.rotation);
    }

    public GameObject Spawn(GameObject obj, Vector3 pos, Quaternion rot)
    {
        GameObject o = Instantiate(obj, pos + 0.001f * spawned * Vector3.forward, rot);
        if (!freebie && wave.Count == 1)
        {
            o.GetComponent<Enemy>().dropWeights[0] = 0;
            freebie = true;
        }
        spawnedEnemies.Add(o);
        spawned++;
        return o;
    }
}
