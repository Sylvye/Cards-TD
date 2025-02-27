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
            stats.ModifyStat("attack_speed", 0.2f);
            stats.ModifyStat("base_damage", 10);
        }
        if (t >= 3)
        {
            stats.ModifyStat("range", 1);
            stats.ModifyStat("explosion_radius", 1);
            stats.ModifyStat("base_damage", 20);
        }
        if (t >= 4)
        {
            stats.ModifyStat("explosion_radius", 1);
            stats.ModifyStat("speed", 1);
            stats.ModifyStat("base_damage", 30);
        }
        if (t >= 5)
        {
            stats.ModifyStat("attack_speed", 0.3f);
            stats.ModifyStat("base_damage", 50);
            stats.ModifyStat("explosion_radius", 1);
        }
    }
}
