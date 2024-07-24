using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Turret : Tower
{
    public float range;
    public float attackSpeed;
    public int damageMultiplier = 1;
    public float projectileSpeedMultiplier = 1;
    public GameObject projectile;
    public float lastShot = -999;

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

    // returns true if it successfully shoots at a target, false if there are no targets in sight
    public virtual bool Shoot()
    {
        GameObject target = GetFirstEnemy();
        if (target != null)
        {
            Vector2 dir = target.transform.position - transform.position;
            GameObject proj = Instantiate(projectile, transform.position, Quaternion.Euler(dir));
            proj.GetComponent<Rigidbody2D>().velocity = projectileSpeedMultiplier * proj.GetComponent<Projectile>().speed * dir.normalized;
            proj.GetComponent<Projectile>().damage *= damageMultiplier;
            proj.GetComponent<Projectile>().parentTower = gameObject;
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
