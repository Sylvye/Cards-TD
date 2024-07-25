using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int hp;
    public int maxhp;
    public float speed;
    public Projectile parentKiller;
    public GameObject[] spawnOnDeath;

    // Update is called once per frame
    void Update()
    {
        transform.position += speed * Time.deltaTime * Vector3.right;
        if (transform.position.x >= 11.5f)
        {
            Main.Damage(hp);
            Destroy(gameObject);
        }
    }

    public bool Damage(int amount)
    {
        hp -= amount;
        if (hp <= 0)
        {
            Destroy(gameObject);
            float spawnOffset = 0;
            foreach (GameObject obj in spawnOnDeath)
            {
                Vector3 spawnPos = transform.position;
                if (obj.CompareTag("Enemy"))
                {
                    spawnPos += Vector3.left * spawnOffset;
                    spawnOffset += 0.5f;
                }
                GameObject instance = Instantiate(obj, spawnPos, obj.transform.rotation);
                instance.GetComponent<Enemy>().Damage(-hp);
            }
            return true;
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
                float spawnOffset = 0;
                foreach (GameObject obj in spawnOnDeath)
                {
                    Vector3 spawnPos = transform.position;
                    if (obj.CompareTag("Enemy"))
                    {
                        spawnPos += Vector3.left * spawnOffset;
                        spawnOffset += 0.5f;
                    }
                    GameObject instance = Instantiate(obj, spawnPos, obj.transform.rotation);
                    instance.GetComponent<Enemy>().Damage(-hp, reference);
                    instance.GetComponent<Enemy>().parentKiller = reference;
                }
                return true;
            }
        }
        return false;
    }
}
