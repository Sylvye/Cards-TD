using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaserTurret : Turret
{
    public override void SetupTierEffects()
    {
        if (tier >= 2)
        {
            // new idea
        }
        if (tier >= 3)
        {
            // new idea
        }
        if (tier >= 4)
        {
            baseDamageBoost++;
        }
        if (tier >= 5)
        {
            projectiles++;
        }
    }
}
