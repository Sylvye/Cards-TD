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
            baseDamageBoost++;
        }
        if (tier >= 3)
        {
            explosionRadiusBoost += 0.1f;
            projectileSpeedMultiplier *= 2;
        }
        if (tier >= 4)
        {
            pierceBoost += 1;
        }
        if (tier >= 5)
        {
            explosionRadiusBoost += 0.3f;
            pierceBoost += 3;
            baseDamageBoost += 3;
        }
    }

    public override float GetRange(int t)
    {
        return t >= 3 ? range + 2 : range;
    }
}
