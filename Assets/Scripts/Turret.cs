using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public abstract class Turret : Tower
{
    // checked!
    public int projectiles = 1;
    public float spread = 10;
    public int pierceBoost = 0;
    public float projectileSpeedMultiplier = 1;
    public float homingSpeed = 0;
    public GameObject projectile;
    public float lastShot = -999;
    public GameObject targetEnemy;

    public override void Awake()
    {
        base.Awake();
        stats.AddStat("projectiles", projectiles);
        stats.AddStat("spread", spread);
        stats.AddStat("pierce", pierceBoost);
        stats.AddStat("mult_speed", projectileSpeedMultiplier);
        stats.AddStat("homing", homingSpeed);
    }

    public void Start()
    {
        ApplyTierEffects();
    }

    private void Update()
    {
        if (lastShot + 1 / stats.GetStat("attack_speed") <= Time.time)
        {
            if (Shoot())
            {
                lastShot = Time.time;
            }
        }
    }

    public void ShootSpread(GameObject projectile, Vector2 direction, int count, float sprd) // spawns across an arc
    {
        float degrees = AngleHelper.VectorToDegrees(direction);
        float startDegrees = degrees + (-sprd * (count - 1) * 0.5f);

        for (int i = 0; i < count; i++)
        {
            float spawnRads = (startDegrees + i * sprd) * Mathf.Deg2Rad;
            Vector2 shootDirection = new(Mathf.Cos(spawnRads), Mathf.Sin(spawnRads));
            Launch(projectile, (Vector2)transform.position + shootDirection * 0.8f, shootDirection);
        }
    }

    private GameObject Launch(GameObject obj, Vector2 spawnPos, Vector2 dir)
    {
        GameObject projectile = Instantiate(obj, spawnPos, Quaternion.LookRotation(Vector3.forward, dir));
        Projectile p = projectile.GetComponent<Projectile>();
        p.angle = AngleHelper.VectorToDegrees(dir.normalized);
        stats.AddToStats(p.stats); // adds all applicable stats over to the projectile
        p.stats.SetStat("damage", GetDamage());
        p.parentTower = gameObject;
        return projectile;
    }

    // returns true if it successfully shoots at a target, false if there are no targets in sight
    public virtual bool Shoot()
    {
        GameObject target = GetFirstEnemy();
        if (target != null)
        {
            Vector2 dir = target.transform.position - transform.position;
            ShootSpread(projectile, dir, (int)stats.GetStat("projectiles"), stats.GetStat("spread"));
            transform.rotation = Quaternion.Euler(0, 0, AngleHelper.VectorToDegrees(dir.normalized)-90);
            return true;
        }
        return false;
    }

    public GameObject GetFirstEnemy()
    {
        RaycastHit2D[] hit = Physics2D.CircleCastAll(transform.position, stats.GetStat("range"), Vector2.zero, 0, Main.enemyLayerMask_);
        if (hit.Length > 0 )
        {
            int firstIndex = 0;
            for (int i = 1; i < hit.Length; i++)
            {
                if (hit[i].transform.position.x > hit[firstIndex].transform.position.x)
                {
                    firstIndex = i;
                }
            }
            targetEnemy = hit[firstIndex].transform.gameObject;
            return hit[firstIndex].transform.gameObject;
        }
        else
        {
            return null;
        }
    }
}
