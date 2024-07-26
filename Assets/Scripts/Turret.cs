using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public abstract class Turret : Tower
{
    public float attackSpeed;
    public int projectiles = 1;
    public float spread = 10;
    public int baseDamageBoost = 0;
    public int pierceBoost = 0;
    public float explosionRadiusBoost = 0;
    public float projectileSpeedMultiplier = 1;
    public float homingSpeed = 0;
    public GameObject projectile;
    public float lastShot = -999;
    public GameObject targetEnemy;

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
        Projectile p = projectile.GetComponent<Projectile>();
        p.angle = AngleHelper.VectorToDegrees(dir.normalized);
        p.speed *= projectileSpeedMultiplier;
        p.damage += baseDamageBoost;
        p.damage = (int)(p.damage * damageMultiplier);
        p.pierce += pierceBoost;
        p.explosionRadius += explosionRadiusBoost;
        p.parentTower = gameObject;
        if (homingSpeed > 0)
            p.homingSpeed = homingSpeed;
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
            targetEnemy = hit[firstIndex].transform.gameObject;
            return hit[firstIndex].transform.gameObject;
        }
        else
        {
            return null;
        }
    }
}
