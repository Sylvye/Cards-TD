using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Turret : Tower
{
    public int range;
    public int damage;
    public int attackSpeed;
    public float projectileSpeed;
    public GameObject projectile;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // returns true if it successfully shoots at a target, false if there are no targets in sight
    public bool Shoot()
    {
        GameObject target = GetFirstEnemy();
        if (target != null)
        {
            Vector2 dir = transform.position - target.transform.position;
            GameObject proj = Instantiate(projectile, transform.position, Quaternion.Euler(dir));
            proj.GetComponent<Rigidbody>().velocity = dir.normalized * projectileSpeed;
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
            return hit[firstIndex].transform.gameObject;
        }
        else
            return null;
    }
}
