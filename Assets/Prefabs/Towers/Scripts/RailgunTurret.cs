using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RailgunTurret : Turret
{
    public override void ApplyTierEffects()
    {
        int t = (int)stats.GetStat("tier");
        if (t >= 2)
        {
            stats.ModifyStat("attack_speed", 0.5f);
            stats.ModifyStat("base_damage", 5);
        }
        if (t >= 3)
        {
            stats.ModifyStat("pierce", 2);
            stats.ModifyStat("range", 2);
            stats.ModifyStat("base_damage", 10);
        }
        if (t >= 4)
        {
            stats.ModifyStat("speed", 2);
            stats.ModifyStat("base_damage", 15);
        }
        if (t >= 5)
        {
            stats.ModifyStat("pierce", 5);
            stats.ModifyStat("base_damage", 20);
        }
    }
}
