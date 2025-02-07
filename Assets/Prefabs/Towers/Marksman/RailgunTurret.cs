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
            stats.AddToStat("attack_speed", 1);
        }
        if (t >= 3)
        {
            stats.AddToStat("pierce", 2);
            stats.AddToStat("range", 2);
        }
        if (t >= 4)
        {
            stats.AddToStat("mult_speed", 2);
            stats.AddToStat("base_damage", 3);
        }
        if (t >= 5)
        {
            stats.AddToStat("pierce", 7);
            stats.AddToStat("base_damage", 4);
        }
    }

    public override float GetRange(int t)
    {
        return t >= 3 ? stats.GetStat("range") + 2 : stats.GetStat("range");
    }
}
