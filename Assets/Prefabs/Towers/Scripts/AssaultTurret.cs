using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AssaultTurret : Turret
{
    public override void ApplyTierEffects()
    {
        int t = (int)stats.GetStat("tier");
        if (t >= 2)
        {
            stats.ModifyStat("range", 1);
        }
        if (t >= 3)
        {
            stats.ModifyStat("base_damage", 5);
            stats.ModifyStat("pierce", 1);
        }
        if (t >= 4)
        {
            stats.ModifyStat("attack_speed", 3);
        }
        if (t >= 5)
        {
            stats.ModifyStat("base_damage", 10);
            stats.ModifyStat("explosion_radius", 0.25f);
        }
    }
}
