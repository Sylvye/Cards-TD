using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int hp;
    public int maxhp;
    public float speed;
    [Header("Drops")]
    public int deathDropPulls;
    public List<GameObject> drops;
    public List<float> dropWeights;

    // Update is called once per frame
    private void Update()
    {
        transform.position += speed * Time.deltaTime * Vector3.right;
        if (transform.position.x >= 11.5f)
        {
            Main.Damage(hp);
            Destroy(gameObject);
            Spawner.spawnedEnemies.Remove(gameObject);
        }
    }

    public bool Damage(int amount)
    {
        hp -= amount;
        if (hp <= 0)
        {
            Destroy(gameObject);
            Spawner.spawnedEnemies.Remove(gameObject);
            PerformDeathDrops(deathDropPulls);
            return true;
        }
        return false;
    }

    //returns true if the attack killed the enemy
    public bool Damage(int amount, Projectile reference)
    {
        hp -= amount;
        if (hp <= 0) // if died
        {
            Destroy(gameObject);
            Spawner.spawnedEnemies.Remove(gameObject);
            PerformDeathDrops(deathDropPulls, AngleHelper.DegreesToVector(reference.angle));
            return true;
        }
        return false;
    }

    public void PerformDeathDrops(int pulls)
    {
        for (int i=0; i<pulls; i++)
        {
            GameObject loot = PullLoot();
            if (loot != null)
            {
                GameObject obj = Instantiate(loot, transform.position, Quaternion.identity);
                ItemDrop objID = loot.GetComponent<ItemDrop>();
                obj.GetComponent<Rigidbody2D>().velocity = AngleHelper.DegreesToVector(UnityEngine.Random.Range(0, 360)).normalized * UnityEngine.Random.Range(5, 11);
            }
        }
    }

    public void PerformDeathDrops(int pulls, Vector2 direction)
    {
        for (int i = 0; i < pulls; i++)
        {
            GameObject loot = PullLoot();
            if (loot != null)
            {
                GameObject obj = Instantiate(loot, transform.position, Quaternion.identity);
                ItemDrop objID = loot.GetComponent<ItemDrop>();
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
        float spd = speed;
        speed = 0;
        yield return new WaitForSeconds(time);
        speed = spd;
    }
}
