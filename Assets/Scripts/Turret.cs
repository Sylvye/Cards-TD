using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public abstract class Turret : Tower
{
    // checked!
    public GameObject projectile;
    public float lastShot = -999;
    public GameObject targetEnemy;

    private void FixedUpdate()
    {
        if (activated && lastShot + 1 / stats.GetStat("attack_speed") <= Time.time)
        {
            if (Action())
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
            ShootProjectile(projectile, (Vector2)transform.position + shootDirection * 0.8f, shootDirection);
        }
    }

    private GameObject ShootProjectile(GameObject obj, Vector2 spawnPos, Vector2 dir)
    {
        GameObject projectile = Instantiate(obj, new Vector3(spawnPos.x, spawnPos.y, 3), Quaternion.LookRotation(Vector3.forward, dir));
        Projectile p = projectile.GetComponent<Projectile>();
        p.angle = AngleHelper.VectorToDegrees(dir.normalized);
        p.stats.AddStats(stats); // adds all applicable stats over to the projectile
        p.stats.SetStat("damage", GetDamage());
        p.type = type;
        p.parentTower = gameObject;
        return projectile;
    }

    // returns true if it successfully locates at a target, false if there are no targets in sight
    public override bool Action()
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
