using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrageTurret : Turret
{
    public override void ApplyTierEffects()
    {
        if (tier >= 2)
        {
            stats.AddToStat("attack_speed", 1);
        }
        if (tier >= 3)
        {
            stats.AddToStat("projectiles", 1);
        }
        if (tier >= 4)
        {
            stats.AddToStat("base_damage", 1);
        }
        if (tier >= 5)
        {
            stats.AddToStat("range", 1.2f);
            stats.AddToStat("attack_speed", 3);
        }
    }

    public override float CalcRange(int t)
    {
        return t >= 5 ? stats.GetStat("range") * 2 : stats.GetStat("range");
    }
}
