using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TowerCard : Card
{
    private Tower prefabTower;
    public int towerIndex;
    public float hitboxRadius;
    [Header("Augmented Stats")]
    public int flatDamage;
    public float attackSpeed;
    public int pierce;
    public float range;
    public int projectiles;
    public float projectileSpeedMult;
    public float homingSpeed;
    public float spread;
    public float explosionRadius;

    public override void Start()
    {
        base.Start();
        prefabTower = spawnable.GetComponent<Tower>();
    }

    public override GameObject OnPlay()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        GameObject obj = Instantiate(spawnable, new Vector3(mousePos.x, mousePos.y, -2), Quaternion.identity);
        Tower tower = obj.GetComponent<Tower>();
        tower.tier = tier;
        tower.range += range;
        tower.damage += flatDamage;
        tower.attackSpeed += attackSpeed;
        if (obj.TryGetComponent(out Turret turret))
        {
            turret.spread -= spread;
            turret.homingSpeed += homingSpeed;
            turret.projectiles += projectiles;
            turret.projectileSpeedMultiplier += projectileSpeedMult;
            turret.pierceBoost += pierce;
            turret.explosionRadius += explosionRadius;
        } else if (obj.TryGetComponent(out TaserTurret taserTurret))
        {
            taserTurret.projectiles += projectiles;
        }

        tower.LoadSprite(towerIndex);

        return obj;
    }

    public float GetReticleRadius()
    {
        return prefabTower.GetRange(tier) + range;
    }

    public override string GetName()
    {
        return prefabTower.name + " T" + tier;
    }

    public override Sprite GetSprite()
    {
        return Resources.LoadAll<Sprite>("CardPack")[towerIndex * 5 + tier - 1];
    }
}
