using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Turret : Tower
{
    public int tier = 1;
    public float range;
    public float attackSpeed;
    public int projectiles = 1;
    public float spread = 10;
    public int damageMultiplier = 1;
    public int baseDamageBoost = 0;
    public int pierceBoost = 0;
    public int chainBoost = 0;
    public float explosionRadiusBoost = 0;
    public float projectileSpeedMultiplier = 1;
    public GameObject projectile;
    public float lastShot = -999;

    private void Start()
    {
        SetupTierEffects();
    }

    void Update()
    {
        if (lastShot + 1 / attackSpeed <= Time.time)
        {
            if (Shoot())
            {
                lastShot = Time.time;
            }
        }
    }

    public virtual void SetupTierEffects()
    {

    }

    public void ShootSpread(GameObject projectile, Vector2 direction, int count, float spread) // spawns across an arc
    {
        float degrees = AngleHelper.VectorToDegrees(direction);
        float startDegrees = degrees + (-spread * (count - 1) * 0.5f);

        for (int i = 0; i < count; i++)
        {
            float spawnRads = (startDegrees + i * spread) * Mathf.Deg2Rad;
            Vector2 shootDirection = new(Mathf.Cos(spawnRads), Mathf.Sin(spawnRads));
            Launch(projectile, (Vector2)transform.position + shootDirection * 0.8f, shootDirection);
        }
    }

    private GameObject Launch(GameObject obj, Vector2 spawnPos, Vector2 dir)
    {
        GameObject projectile = Instantiate(obj, spawnPos, Quaternion.LookRotation(Vector3.forward, dir));
        projectile.GetComponent<Rigidbody2D>().velocity = projectileSpeedMultiplier * projectile.GetComponent<Projectile>().speed * dir.normalized;
        Projectile p = projectile.GetComponent<Projectile>();
        p.damage += baseDamageBoost;
        p.damage *= damageMultiplier;
        p.pierce += pierceBoost;
        p.chain += chainBoost;
        p.explosionRadius += explosionRadiusBoost;
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
            ShootSpread(projectile, dir, projectiles, spread);
            return true;
        }
        return false;
    }

    public GameObject GetFirstEnemy()
    {
        RaycastHit2D[] hit = Physics2D.CircleCastAll(transform.position, range, Vector2.zero, 0, Main.enemyLayerMask_);
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
            Debug.DrawLine(transform.position, hit[firstIndex].transform.position, Color.green, 0.5f);
            return hit[firstIndex].transform.gameObject;
        }
        else
        {

            Debug.DrawLine(transform.position + new Vector3(0.5f, 0.5f, 0), transform.position + new Vector3(-0.5f, -0.5f, 0), Color.red);
            Debug.DrawLine(transform.position + new Vector3(0.5f, -0.5f, 0), transform.position + new Vector3(-0.5f, 0.5f, 0), Color.red);
            return null;
        }
    }
}
