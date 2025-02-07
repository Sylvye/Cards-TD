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
            stats.AddToStat("range", 1);
        }
        if (t >= 3)
        {
            stats.AddToStat("base_damage", 1);
        }
        if (t >= 4)
        {
            stats.AddToStat("attack_speed", 3);
        }
        if (t >= 5)
        {
            stats.AddToStat("base_damage", 1);
            stats.AddToStat("explosion_radius", 0.25f);
        }
    }

    public override float GetRange(int t)
    {
        return t >= 2 ? stats.GetStat("range") + 1 : stats.GetStat("range");
    }
}
