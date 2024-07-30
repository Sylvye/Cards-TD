using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Augment : MonoBehaviour
{
    public string type;
    public int flatDamage;
    public float attackSpeed;
    public float range;
    public int pierce;
    public int projectiles;
    public float projectileSpeedMult; // ADDITIVE
    public float homingSpeed;
    public float spread;

    void ApplyEffect(Tower tower)
    {
        tower.range += range;
        tower.damage += flatDamage;
        tower.attackSpeed += attackSpeed;
        if (TryGetComponent(out Turret turret))
        {
            turret.spread -= spread;
            turret.homingSpeed += homingSpeed;
            turret.projectiles += projectiles;
            turret.projectileSpeedMultiplier += projectileSpeedMult;
            turret.pierceBoost += pierce;
        }
    }
}
