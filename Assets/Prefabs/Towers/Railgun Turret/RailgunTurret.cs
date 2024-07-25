using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RailgunTurret : Turret
{
    public override void SetupTierEffects()
    {
        if (tier >= 2)
        {
            attackSpeed *= 1.5f;
        }
        if (tier >= 3)
        {
            pierceBoost += 2;
        }
        if (tier >= 4)
        {
            projectileSpeedMultiplier *= 2f;
            baseDamageBoost += 3;
        }
        if (tier >= 5)
        {
            pierceBoost += 8;
            baseDamageBoost += 4;
        }
    }
}
