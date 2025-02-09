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
            stats.ModifyStat("range", 1);
            stats.ModifyStat("base_damage", 1);
        }
        if (t >= 3)
        {
            stats.ModifyStat("speed", 1);
            stats.ModifyStat("attack_speed", 0.2f);
        }
        if (t >= 4)
        {
            stats.ModifyStat("explosion_radius", 0.25f);
            stats.ModifyStat("speed", 1);
        }
        if (t >= 5)
        {
            stats.ModifyStat("attack_speed", 0.3f);
            stats.ModifyStat("base_damage", 3);
        }
    }
}
