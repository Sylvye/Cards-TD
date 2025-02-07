using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketTurret : Turret
{
    public override void ApplyTierEffects()
    {
        int t = (int)stats.GetStat("tier");
        if (t >= 2)
        {
            stats.AddToStat("range", 1);
            stats.AddToStat("base_damage", 1);
        }
        if (t >= 3)
        {
            stats.AddToStat("mult_speed", 1);
        }
        if (t >= 4)
        {
            stats.AddToStat("attack_speed", 0.2f);
            stats.AddToStat("mult_speed", 1);
        }
        if (t >= 5)
        {
            stats.AddToStat("pierce", 1);
            stats.AddToStat("explosion_radius", 0.25f);
            stats.AddToStat("attack_speed", 0.3f);
            stats.AddToStat("base_damage", 3);
        }
    }

    public override float GetRange(int t)
    {
        return t >= 2 ? stats.GetStat("range") + 1 : stats.GetStat("range");
    }
}
