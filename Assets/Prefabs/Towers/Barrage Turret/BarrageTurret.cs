using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrageTurret : Turret
{
    public override void InitTierEffects()
    {
        if (tier >= 2)
        {
            attackSpeed *= 1.5f;
        }
        if (tier >= 3)
        {
            projectiles++;
        }
        if (tier >= 4)
        {
            damage++;
        }
        if (tier >= 5)
        {
            range *= 2;
            attackSpeed *= 2f;
        }
    }

    public override float GetRange(int t)
    {
        return t >= 5 ? range * 2 : range;
    }
}
