using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RailgunTurret : Turret
{
    public override void ApplyTierEffects()
    {
        if (tier >= 2)
        {
            stats.AddToStat("attack_speed", 1);
        }
        if (tier >= 3)
        {
            stats.AddToStat("pierce", 2);
            stats.AddToStat("range", 2);
        }
        if (tier >= 4)
        {
            stats.AddToStat("speed", 2);
            stats.AddToStat("base_damage", 3);
        }
        if (tier >= 5)
        {
            stats.AddToStat("pierce", 7);
            stats.AddToStat("base_damage", 4);
        }
    }

    public override float CalcRange(int t)
    {
        return t >= 3 ? stats.GetStat("range") + 2 : stats.GetStat("range");
    }
}
