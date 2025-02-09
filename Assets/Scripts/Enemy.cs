using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Drops")]
    public int pulls;
    public List<GameObject> drops;
    public List<float> dropWeights;
    private float lastStun;
    private float stunDuration; // make stun work in Update() instead of in a coroutine

    [NonSerialized]
    public Stats stats;

    private void Awake()
    {
        stats = GetComponent<Stats>();
    }

    // Update is called once per frame
    private void Update()
    {
        transform.position += stats.GetStat("speed") * Time.deltaTime * Vector3.right;
        if (transform.position.x >= 11.5f)
        {
            Main.Damage((int)stats.GetStat("hp"));
            Destroy(gameObject);
            Spawner.spawnedEnemies.Remove(gameObject);
        }
    }

    public bool Damage(int amount)
    {
        stats.ModifyStat("hp", amount, Stats.Operation.Subtract);
        if (stats.GetStat("hp") <= 0) // death
        {
            Destroy(gameObject);
            Spawner.spawnedEnemies.Remove(gameObject);
            RollLoot(pulls);
            return true;
        }
        return false;
    }

    //returns true if the attack killed the enemy
    public bool Damage(int amount, Projectile reference)
    {
        stats.ModifyStat("hp", amount, Stats.Operation.Subtract);
        if (stats.GetStat("hp") <= 0) // death
        {
            Destroy(gameObject);
            Spawner.spawnedEnemies.Remove(gameObject);
            RollLoot(pulls, AngleHelper.DegreesToVector(reference.angle));
            return true;
        }
        return false;
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
        StartCoroutine(StunC(time));
    }

    private IEnumerator StunC(float time)
    {
        float spd = stats.GetStat("speed");
        stats.SetStat("speed", 0);
        yield return new WaitForSeconds(time);
        stats.SetStat("speed", spd);
    }
}
