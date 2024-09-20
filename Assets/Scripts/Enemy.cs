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
    public Projectile parentKiller;
    public GameObject[] children;
    [Header("Drops")]
    public int deathDropPulls;
    public List<GameObject> drops;
    public List<float> dropWeights;

    // Update is called once per frame
    void Update()
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
            if (children.Length > 0)
            {
                float spawnOffset = 0;
                for (int i = 0; i < children.Length; i++)
                {
                    GameObject obj = children[i];
                    Vector3 spawnPos = transform.position;
                    if (obj.CompareTag("Enemy"))
                    {
                        spawnPos += Vector3.left * spawnOffset;
                        spawnOffset += 0.5f;
                    }
                    GameObject instance = Instantiate(obj, spawnPos, obj.transform.rotation);
                    Enemy child = instance.GetComponent<Enemy>();
                    child.Damage(-hp / children.Length);
                    if (i == 0)
                    {
                        child.drops = drops;
                        child.dropWeights = dropWeights;
                    }
                    else
                    {
                        child.drops = new List<GameObject>();
                        child.dropWeights = new List<float>();
                    }
                }
            }
            else
            {
                PerformDeathDrops(deathDropPulls);
                return true;
            }
        }
        return false;
    }

    public bool Damage(int amount, Projectile reference)
    {
        if (parentKiller != reference)
        {
            hp -= amount;
            if (hp <= 0)
            {
                Destroy(gameObject);
                Spawner.spawnedEnemies.Remove(gameObject);
                if (children.Length > 0)
                {
                    float spawnOffset = 0;
                    for (int i=0; i<children.Length; i++)
                    {
                        GameObject obj = children[i];
                        Vector3 spawnPos = transform.position;
                        if (obj.CompareTag("Enemy"))
                        {
                            spawnPos += Vector3.left * spawnOffset;
                            spawnOffset += 0.5f;
                        }
                        GameObject instance = Instantiate(obj, spawnPos, obj.transform.rotation);
                        Enemy child = instance.GetComponent<Enemy>();
                        child.Damage(-hp / children.Length, reference);
                        child.parentKiller = reference;
                        if (i == 0)
                        {
                            child.drops = drops;
                            child.dropWeights = dropWeights;
                        }
                        else
                        {
                            child.drops = new List<GameObject>();
                            child.dropWeights = new List<float>();
                        }
                        if (speed == 0)
                        {
                            child.Stun(0.1f);
                        }
                    }
                }
                else
                {
                    PerformDeathDrops(deathDropPulls, AngleHelper.DegreesToVector(reference.angle));
                    return true;
                }
            }
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

    IEnumerator StunC(float time)
    {
        float spd = speed;
        speed = 0;
        yield return new WaitForSeconds(time);
        speed = spd;
    }
}
