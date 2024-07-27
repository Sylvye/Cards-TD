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
    public GameObject[] children;

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
            float spawnOffset = 0;
            foreach (GameObject obj in children)
            {
                Vector3 spawnPos = transform.position;
                if (obj.CompareTag("Enemy"))
                {
                    spawnPos += Vector3.left * spawnOffset;
                    spawnOffset += 0.5f;
                }
                GameObject instance = Instantiate(obj, spawnPos, obj.transform.rotation);
                instance.GetComponent<Enemy>().Damage(-hp / children.Length);
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
                Spawner.spawnedEnemies.Remove(gameObject);
                float spawnOffset = 0;
                foreach (GameObject obj in children)
                {
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
                    if (speed == 0)
                    {
                        child.Stun(0.1f);
                    }
                }
                return true;
            }
        }
        return false;
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
