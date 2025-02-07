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
    public int projectiles;
    public float spread;
    public float range;
    public float attackSpeed;
    public int baseDamage;
    public int pierce;
    public float projectileSpeedMult;
    public float homingSpeed;
    public float explosionRadius;

    public override void Awake()
    {
        base.Awake();
        Debug.Log(name + " Awakened");
        prefabTower = spawnable.GetComponent<Tower>();
        stats.AddStat("base_damage", baseDamage);
        stats.AddStat("attack_speed", attackSpeed);
        stats.AddStat("pierce", pierce);
        stats.AddStat("range", range);
        stats.AddStat("projectiles", projectiles);
        stats.AddStat("mult_speed", projectileSpeedMult);
        stats.AddStat("homing", homingSpeed);
        stats.AddStat("spread", spread);
        stats.AddStat("explosion_radius", explosionRadius);
    }

    public override GameObject OnPlay()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        GameObject obj = Instantiate(spawnable, new Vector3(mousePos.x, mousePos.y, -2), Quaternion.identity);
        Tower tower = obj.GetComponent<Tower>();
        stats.AddToStats(tower.stats);
        tower.stats.SetStat("tier", stats.GetStat("tier"));
        //if (obj.TryGetComponent(out Turret turret))
        //{
        //    turret.spread -= spread;
        //    turret.projectiles += projectiles;
        //    turret.explosionRadius += explosionRadius;
        //} else if (obj.TryGetComponent(out TaserTurret taserTurret))
        //{
        //    taserTurret.projectiles += projectiles;
        //}

        tower.LoadSprite(towerIndex);

        return obj;
    }

    public float GetReticleRadius()
    {
        // PREFAB!! do NOT replace with a GetStat()!!!
        // base range + augmented range
        return prefabTower.GetRange((int)stats.GetStat("tier")) + stats.GetStat("range");
    }

    public override string GetName()
    {
        return prefabTower.name + " T" + stats.GetStat("tier");
    }

    public override Sprite GetSprite()
    {
        return Resources.LoadAll<Sprite>("CardPack")[towerIndex * 5 + (int)stats.GetStat("tier") - 1];
    }
}
