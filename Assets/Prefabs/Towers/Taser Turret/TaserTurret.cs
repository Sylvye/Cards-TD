using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public class TaserTurret : Tower
{
    public GameObject laser;
    public int damage;
    public float attackSpeed;
    public int projectiles = 1;
    public float lastShot = -999;
    public GameObject FX;

    public float stunTime = 0;

    private void Start()
    {
        SetupTierEffects();
    }

    void Update()
    {
        if (lastShot + 1 / attackSpeed <= Time.time)
        {
            Shoot();
        }
    }
    
    public void Shoot()
    {
        GameObject[] targets = GetTargets();
        foreach (GameObject target in targets)
        {
            if (target != null)
            {
                GameObject laserObj = Instantiate(laser, Vector2.zero, Quaternion.identity);
                LineRenderer lr = laserObj.GetComponent<LineRenderer>();
                Enemy e = target.GetComponent<Enemy>();

                lr.SetPosition(0, (Vector2)transform.position);
                lr.SetPosition(1, target.transform.position);
                Destroy(laserObj, 0.1f);

                Vector3 randomOffset = new(Random.Range(-0.1f, 0.1f), Random.Range(-0.1f, 0.1f), -1);
                Instantiate(FX, target.transform.position + randomOffset, Quaternion.identity); // spawns spark effect

                if (stunTime > 0)
                    e.Stun(stunTime);
                e.Damage((int)(damage * damageMultiplier));
            }
        }

        lastShot = Time.time;
    }

    public GameObject[] GetTargets()
    {
        List<RaycastHit2D> hit = new(Physics2D.CircleCastAll(transform.position, range, Vector2.zero, 0, Main.enemyLayerMask_));
        List<GameObject> output = new();

        for (int i=0; i<projectiles; i++)
        {
            if (hit.Count > 0)
            {
                int firstIndex = 0;
                for (int j = 0; j < hit.Count; j++)
                {
                    if (hit[j].transform.position.x > hit[firstIndex].transform.position.x)
                    {
                        firstIndex = j;
                    }
                }
                output.Add(hit[firstIndex].collider.gameObject);
                hit.RemoveAt(firstIndex);
            }
        }

        return output.ToArray();
    }

    public void SetupTierEffects()
    {
        if (tier >= 2)
        {
            projectiles += 2;
            stunTime += 0.2f;
        }
        if (tier >= 3)
        {
            projectiles += 2;
            attackSpeed *= 1.66f;
        }
        if (tier >= 4)
        {
            projectiles += 2;
            damage++;
        }
        if (tier >= 5)
        {
            projectiles += 2;
            damage++;
            stunTime += 0.1f;
            range += 3;
        }
    }

    public override float GetRange(int t)
    {
        return t >= 5 ? range + 3 : range;
    }
}
