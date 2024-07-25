using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrageTurret : Turret
{
    public override void SetupTierEffects()
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
            baseDamageBoost++;
        }
        if (tier >= 5)
        {
            range *= 2;
            attackSpeed *= 2f;
        }
    }
}
