using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrageTurret : Turret
{
    public override void ApplyTierEffects()
    {
        int t = (int)stats.GetStat("tier");
        if (t >= 2)
        {
            stats.ModifyStat("attack_speed", 1);
            stats.ModifyStat("range", 0.5f);
        }
        if (t >= 3)
        {
            stats.ModifyStat("projectiles", 1);
        }
        if (t >= 4)
        {
            stats.ModifyStat("base_damage", 1);
        }
        if (t >= 5)
        {
            stats.ModifyStat("attack_speed", 3);
            stats.ModifyStat("spread", 10, Stats.Operation.Subtract);
        }
    }
}
