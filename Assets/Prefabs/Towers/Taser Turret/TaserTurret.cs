using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class TaserTurret : Turret
{
    public float stunTime = 0;
    public override bool Shoot()
    {
        GameObject[] targets = new GameObject[projectiles];
        for (int i = 0; i < projectiles; i++)
        {
            GameObject target = GetFirstEnemy(targets);
            if (target != null)
            {
                GameObject obj = Instantiate(projectile, Vector2.zero, Quaternion.identity);
                LineRenderer l = obj.GetComponent<LineRenderer>();

                l.SetPosition(0, (Vector2)transform.position);
                l.SetPosition(1, target.transform.position);
                Destroy(obj, 0.1f);

                target.GetComponent<Enemy>().Damage(baseDamageBoost * damageMultiplier);
                if (stunTime > 0)
                {
                    target.GetComponent<Enemy>().Stun(stunTime);
                }
            }
            targets[i] = target;
        }

        return targets[0] != null;
    }

    public override void SetupTierEffects()
    {
        if (tier >= 2)
        {
            projectiles++;
        }
        if (tier >= 3)
        {
            stunTime += 0.1f;
        }
        if (tier >= 4)
        {
            baseDamageBoost++;
        }
        if (tier >= 5)
        {
            projectiles++;
            stunTime += 0.1f;
        }
    }
}
