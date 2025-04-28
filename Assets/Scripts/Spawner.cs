using AYellowpaper.SerializedCollections;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Spawner : MonoBehaviour
{
    public static Spawner main;
    public static List<GameObject> spawnedEnemies = new(); // all spawned enemies, including from other spawners
    public List<GameObject> enemies; // reference to match with enemy indexes in wave (below)
    public List<int> wave; // list of indexes leading to which enemy to spawn when
    public bool[] loot;
    public int maxLoot = 3;
    public int minLoot = 1;
    public int waveIndex = 0; // the current index in wave of the enemy to spawn
    private bool active = false; // if the spawner is active
    public bool complete = false; // if the wave is over
    private int spawned; // how many enemies spawned so far this wave
    [SerializeField]
    private float cooldown; // the time between spawning enemies
    private float lastSpawnTime;
    private static bool freebie = false; // whether or not a lootbag has dropped yet

    [SerializedDictionary("Name", "Stat")]
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
                if (Time.time >= lastSpawnTime + cooldown)
                {
                    Send(wave[waveIndex]);
                    waveIndex++;
                    lastSpawnTime = Time.time;
                }
            }
            else
            {
                Complete();
            }
        }
    }

    public void StartWave()
    {
        UpdateWave();
        AssignLoot();
        active = true;
        cooldown = 1 / Main.enemyStats.GetStat("wave_density");
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
        spawned = 0;
        waveIndex = 0;
    }

    public bool IsStageCleared()
    {
        return StageController.currentStage == StageController.Stage.Battle && complete && spawnedEnemies.Count == 0;
    }

    public void Send(int tier)
    {
        Enemy e = Spawn(enemies[tier - 1], transform.position, enemies[tier - 1].transform.rotation).GetComponent<Enemy>();
        float speedMult = stats.GetStat(Enemy.TierToType(tier) + "_enemy_speed_mult");
        float hp = e.stats.GetStat("max_hp") * stats.GetStat("hp_mult");
        float sizeMult = stats.GetStat(Enemy.TierToType(tier) + "_enemy_hp_mult");
        if (sizeMult < 0)
            sizeMult = 1;
        hp *= sizeMult;
        hp += stats.GetStat("flat_hp");
        e.size = (Enemy.Size)tier;
        e.stats.SetStat("max_hp", hp);

        e.stats.ModifyStat("speed", speedMult, Stat.Operation.Multiply);
        e.stats.ModifyStat("shield", stats.GetStat("shield"));
        e.stats.ModifyStat("regeneration", stats.GetStat("regeneration"));
        e.stats.ModifyStat("resistance", stats.GetStat("resistance"), Stat.Operation.Multiply);
        e.stats.ModifyStat("desperation", stats.GetStat("desperation"), Stat.Operation.Multiply);
        
        e.pulls = loot[waveIndex] ? 1 : 0;
    }

    public GameObject Spawn(GameObject obj, Vector3 pos, Quaternion rot)
    {
        GameObject enemy = Instantiate(obj, pos + 0.001f * spawned * Vector3.forward, rot);
        Enemy e = enemy.GetComponent<Enemy>();
        if (!freebie && waveIndex == wave.Count)
        {
            e.dropWeights[0] = 0;
            freebie = true;
        }
        spawnedEnemies.Add(enemy);
        spawned++;
        return enemy;
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

    public void AssignLoot()
    {
        loot = new bool[wave.Count];
        for (int i=minLoot; i<=maxLoot; i++)
        {
            int li = UnityEngine.Random.Range(0, wave.Count);
            while (i < wave.Count)
            {
                if (!loot[li]) // if there is no loot, give it loot. otherwise keep trying
                {
                    loot[li] = true;
                    break;
                }
                li = UnityEngine.Random.Range(0, wave.Count);
            }
        }
    }
}
