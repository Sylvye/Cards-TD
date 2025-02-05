using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Spawner : MonoBehaviour
{
    public static Spawner main;
    public static List<GameObject> spawnedEnemies = new List<GameObject>();
    public List<GameObject> enemies;
    public List<GameObject> wave;
    public int waveIndex = 0;
    public bool active = true;
    public bool complete = false;
    private int spawned; // how many enemies spawned this wave
    private float cooldown = 0;
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
            if (waveIndex < wave.Count)
            {
                cooldown -= Time.deltaTime;

                if (cooldown <= 0)
                {
                    if (wave[waveIndex] != null)
                    {
                        cooldown = 1 / Main.enemyStats.GetStat("wave_density");
                        Spawn(wave[waveIndex], transform.position, wave[waveIndex++].transform.rotation);
                    }
                    else
                    {
                        cooldown = 1 / Main.enemyStats.GetStat("wave_density");
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
        waveIndex = 0;
    }

    public bool IsStageComplete()
    {
        return StageController.currentStage == StageController.Stage.Battle && complete && spawnedEnemies.Count == 0;
    }

    public void Send(int tier)
    {
        Spawn(enemies[tier - 1], transform.position, enemies[tier - 1].transform.rotation);
    }

    public GameObject Spawn(GameObject obj, Vector3 pos, Quaternion rot)
    {
        GameObject o = Instantiate(obj, pos + 0.001f * spawned * Vector3.forward, rot);
        if (!freebie && waveIndex == wave.Count)
        {
            o.GetComponent<Enemy>().dropWeights[0] = 0;
            freebie = true;
        }
        spawnedEnemies.Add(o);
        spawned++;
        return o;
    }
}
