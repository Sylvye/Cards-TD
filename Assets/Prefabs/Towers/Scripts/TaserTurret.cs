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

    private void FixedUpdate()
    {
        if (activated && lastShot + 1 / stats.GetStat("attack_speed") <= Time.time)
        {
            Action();
        }
    }
    
    public override bool Action()
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

                if (stats.GetStat("explosion_radius") > 0)
                {
                    RaycastHit2D[] hit = Physics2D.CircleCastAll(e.transform.position, stats.GetStat("explosion_radius"), Vector2.zero, 0, Main.enemyLayerMask_);
                    foreach (RaycastHit2D rayC in hit)
                    {
                        GameObject obj = rayC.collider.gameObject;
                        obj.GetComponent<Enemy>().Damage(GetDamage());
                    }
                }

                e.Damage(GetDamage());
            }
        }

        lastShot = Time.time;
        return true; // always hits
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
        int t = (int)stats.GetStat("tier");
        if (t >= 2)
        {
            stats.ModifyStat("projectiles", 2);
            stats.ModifyStat("stun", 0.05f);
            stats.ModifyStat("base_damage", 5);
        }
        if (t >= 3)
        {
            stats.ModifyStat("projectiles", 2);
            stats.ModifyStat("attack_speed", 0.6f);
            stats.ModifyStat("base_damage", 5);
        }
        if (t >= 4)
        {
            stats.ModifyStat("projectiles", 2);
            stats.ModifyStat("base_damage", 10);
        }
        if (t >= 5)
        {
            stats.ModifyStat("projectiles", 1);
            stats.ModifyStat("base_damage", 10);
            stats.ModifyStat("stun", 0.05f);
            stats.ModifyStat("range", 3);
        }
    }
}
