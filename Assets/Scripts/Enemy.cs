using AYellowpaper.SerializedCollections;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public enum Size
    {
        Small,
        Medium, 
        Large,
        Other
    }
    public Size size;
    [Header("Drops")]
    public int pulls;
    public List<GameObject> drops;
    public List<float> dropWeights;
    private float stunEnd;
    
    [SerializedDictionary("Name", "Stat")]
    public Stats stats;

    private void Awake()
    {
        stats = GetComponent<Stats>();
    }

    private void Start()
    {
        stats.SetStat("hp", stats.GetStat("max_hp"));
        StartCoroutine(Regenerate());
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (Time.time > stunEnd)
        {
            float boost = (stats.GetStat("desperation") - 1) * (1 - stats.GetStat("hp") / stats.GetStat("max_hp")) + 1;
            transform.position += stats.GetStat("speed") * boost * 0.01f * Vector3.right;
            if (transform.position.x >= 11.5f)
            {
                Main.Damage((int)stats.GetStat("hp"));
                Destroy(gameObject);
                Spawner.spawnedEnemies.Remove(gameObject);
            }
        }
    }

    // returns any excess damage
    public int Damage(int amount)
    {
        amount *= 1 - (int)stats.GetStat("resistance");
        amount -= (int)stats.GetStat("shield");
        if (amount <= 0)
            return 0;
        stats.ModifyStat("hp", amount, Stat.Operation.Subtract);
        if (stats.GetStat("hp") <= 0) // death
        {
            Destroy(gameObject);
            Spawner.spawnedEnemies.Remove(gameObject);
            RollLoot(pulls);
            return -(int)stats.GetStat("hp");
        }
        return 0;
    }

    // returns any excess damage
    public int Damage(int amount, Projectile reference)
    {
        amount *= 1 - (int)stats.GetStat("resistance");
        amount -= (int)stats.GetStat("shield");
        if (amount <= 0)
            return 0;
        stats.ModifyStat("hp", amount, Stat.Operation.Subtract);
        if (stats.GetStat("hp") <= 0) // death
        {
            Destroy(gameObject);
            Spawner.spawnedEnemies.Remove(gameObject);
            RollLoot(pulls, AngleHelper.DegreesToVector(reference.angle));
            return -(int)stats.GetStat("hp");
        }
        return 0;
    }

    public void RollLoot(int pulls)
    {
        for (int i=0; i<pulls; i++)
        {
            GameObject loot = PullLoot();
            if (loot != null)
            {
                GameObject obj = Instantiate(loot, transform.position, Quaternion.identity);
                LootBag objID = loot.GetComponent<LootBag>();
                obj.GetComponent<Rigidbody2D>().velocity = AngleHelper.DegreesToVector(UnityEngine.Random.Range(0, 360)).normalized * UnityEngine.Random.Range(5, 11);
            }
        }
    }

    public void RollLoot(int pulls, Vector2 direction)
    {
        for (int i = 0; i < pulls; i++)
        {
            GameObject loot = PullLoot();
            if (loot != null)
            {
                GameObject obj = Instantiate(loot, transform.position, Quaternion.identity);
                LootBag objID = loot.GetComponent<LootBag>();
                obj.GetComponent<Rigidbody2D>().velocity = direction.normalized * 10;
            }
        }
    }

    // performs one pull from lootpool, returns object (null if nothing)
    public GameObject PullLoot()
    {
        if (drops.Count == 0) return null;
        return drops[WeightedRandom.SelectWeightedIndex(dropWeights)];
    }

    public void Stun(float time)
    {
        stunEnd = Time.time + time;
    }

    public bool IsAlive()
    {
        return stats.GetStat("hp") > 0;
    }

    public static string TierToType(int tier)
    {
        return tier switch
        {
            1 => "small",
            2 => "medium",
            3 => "large",
            _ => ""
        };
    }

    private IEnumerator Regenerate()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            if (stats.GetStat("regeneration") + stats.GetStat("hp") <= stats.GetStat("max_hp"))
            {
                stats.ModifyStat("hp", stats.GetStat("regeneration"));
            }
        }
    }
}
