using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public class TaserTurret : Tower
{
    // checked!
    public GameObject laser;
    public GameObject FX;
    private float lastShot = -999;

    public override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        ApplyTierEffects();
    }

    private void Update()
    {
        if (lastShot + 1 / stats.GetStat("attack_speed") <= Time.time)
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

                if (stats.GetStat("stun") > 0)
                    e.Stun(stats.GetStat("stun"));
                e.Damage(GetDamage());
            }
        }

        lastShot = Time.time;
    }

    public GameObject[] GetTargets()
    {
        List<RaycastHit2D> hit = new(Physics2D.CircleCastAll(transform.position, stats.GetStat("range"), Vector2.zero, 0, Main.enemyLayerMask_));
        List<GameObject> output = new();

        for (int i=0; i< stats.GetStat("projectiles"); i++)
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

    public override void ApplyTierEffects()
    {
        if (tier >= 2)
        {
            stats.AddToStat("projectiles", 2);
            stats.AddToStat("stun", 0.2f);
        }
        if (tier >= 3)
        {
            stats.AddToStat("projectiles", 2);
            stats.AddToStat("attack_speed", 0.6f);
        }
        if (tier >= 4)
        {
            stats.AddToStat("projectiles", 2);
            stats.AddToStat("base_damage", 1);
        }
        if (tier >= 5)
        {
            stats.AddToStat("projectiles", 2);
            stats.AddToStat("base_damage", 1);
            stats.AddToStat("stun", 1);
            stats.AddToStat("range", 3);
        }
    }

    public override float CalcRange(int t)
    {
        return t >= 5 ? stats.GetStat("range") + 3 : stats.GetStat("range");
    }
}
