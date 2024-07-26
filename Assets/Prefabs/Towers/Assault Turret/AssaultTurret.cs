using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AssaultTurret : Turret
{
    public override void SetupTierEffects()
    {
        if (tier >= 2)
        {
            range++;
        }
        if (tier >= 3)
        {
            baseDamageBoost++;
        }
        if (tier >= 4)
        {
            attackSpeed *= 2;
        }
        if (tier >= 5)
        {
            baseDamageBoost++;
            explosionRadiusBoost += 0.25f;
        }
    }

    public override float GetRange(int t)
    {
        return t >= 1 ? range + 1 : range;
    }
}
