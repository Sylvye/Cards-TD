using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketTurret : Turret
{
    public override void SetupTierEffects()
    {
        if (tier >= 2)
        {
            range++;
            damage++;
        }
        if (tier >= 3)
        {
            projectileSpeedMultiplier *= 2;
        }
        if (tier >= 4)
        {
            attackSpeed *= 1.5f;
            projectileSpeedMultiplier *= 1.5f;
        }
        if (tier >= 5)
        {
            pierceBoost++;
            explosionRadiusBoost += 0.25f;
            attackSpeed *= 1.5f;
            damage += 3;
        }
    }

    public override float GetRange(int t)
    {
        return t >= 2 ? range + 1 : range;
    }
}
