using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Spawner : MonoBehaviour
{
    public static Spawner main;
    public static List<GameObject> spawnedEnemies = new();
    public List<GameObject> enemies;
    public List<int> wave;
    public int waveIndex = 0;
    private bool active = false;
    public bool complete = false;
    private int spawned; // how many enemies spawned this wave
    [SerializeField]
    private float cooldown = 0;
    private bool freebie = false;

    [NonSerialized]
    public Stats stats;

    private void Awake()
    {
        main = this;
        stats = GetComponent<Stats>();
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
                    Send(wave[waveIndex++]);
                    cooldown = 1 / Main.enemyStats.GetStat("wave_density");
                }
            }
            else
            {
                Complete();
            }
        }
    }

    public void SetActive(bool a)
    {
        if (a)
        {
            UpdateWave();
        }
        active = a;
    }

    public bool GetActive()
    {
        return active;
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
        Enemy e = Spawn(enemies[tier - 1], transform.position, enemies[tier - 1].transform.rotation).GetComponent<Enemy>();
        float speedMult = stats.GetStat(TierToType(tier) + "_enemy_speed_mult");
        float hpMult = stats.GetStat(TierToType(tier) + "_enemy_hp_mult");
        e.stats.ModifyStat("speed", speedMult, Stats.Operation.Multiply);
        e.stats.ModifyStat("hp", hpMult, Stats.Operation.Multiply);
        e.stats.ModifyStat("max_hp", hpMult, Stats.Operation.Multiply);
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

    private string TierToType(int tier)
    {
        return tier switch
        {
            1 => "small",
            2 => "medium",
            3 => "large",
            _ => ""
        };
    }

    public void UpdateWave()
    {
        int small = (int)stats.GetStat("small_enemies");
        int medium = (int)stats.GetStat("medium_enemies");
        int large = (int)stats.GetStat("large_enemies");
        for (int i = 0; i < small; i++)
        {
            wave.Add(1);
        }
        for (int i = 0; i < medium; i++)
        {
            wave.Add(2);
        }
        for (int i = 0; i < large; i++)
        {
            wave.Add(3);
        }
        stats.SetStat("small_enemies", 0);
        stats.SetStat("medium_enemies", 0);
        stats.SetStat("large_enemies", 0);
    }
}
