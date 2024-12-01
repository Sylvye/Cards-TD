using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AssaultTurret : Turret
{
    public override void InitTierEffects()
    {
        if (tier >= 2)
        {
            range++;
        }
        if (tier >= 3)
        {
            damage++;
        }
        if (tier >= 4)
        {
            attackSpeed *= 2;
        }
        if (tier >= 5)
        {
            damage++;
            explosionRadiusBoost += 0.25f;
        }
    }

    public override float GetRange(int t)
    {
        return t >= 2 ? range + 1 : range;
    }
}
