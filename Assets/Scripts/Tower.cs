using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Tower : MonoBehaviour
{
    // checked!
    public enum Type
    {
        Kinetic,
        Energy
    }
    public Stats stats;
    public int tier;
    public Type type = Type.Kinetic;
    public int base_damage;
    public float attackSpeed;
    public float damageMultiplier;
    public float range;
    public float explosionRadius;

    public virtual void Awake()
    {
        Debug.Log(name + " Awakened");
        stats = new();
        stats.AddStat("tier", tier);
        stats.AddStat("base_damage", base_damage);
        stats.AddStat("damage_mult", damageMultiplier);
        stats.AddStat("attack_speed", attackSpeed);
        stats.AddStat("range", range);
        stats.AddStat("explosion_radius", explosionRadius);
    }

    public void LoadSprite(int towerIndex)
    {
        GetComponent<SpriteRenderer>().sprite = Resources.LoadAll<Sprite>("TowerArt")[towerIndex * 5 + (int)stats.GetStat("tier") - 1];
    }

    public float GetRange()
    {
        return GetRange((int)stats.GetStat("tier"));
    }

    public virtual float GetRange(int t)
    {
        return stats.GetStat("range");
    }

    public abstract void ApplyTierEffects();

    private void OnMouseEnter()
    {
        if (!Card.isCardSelected && !Spawner.main.IsStageComplete())
        {
            Main.towerRangeReticle_.transform.position = transform.position;
            Main.towerRangeReticle_.transform.localScale = GetRange() * 2 * Vector3.one + Vector3.forward * -6;
        }
    }

    private void OnMouseExit()
    {
        if (!Card.isCardSelected)
        {
            Main.towerRangeReticle_.transform.position = new Vector3(4, 10, 0);
            Main.towerRangeReticle_.transform.localScale = Vector2.one;
        }
    }

    public virtual int GetDamage()
    {
        float typeDamage = type == Type.Kinetic ? Main.playerStats.GetStat("kinetic_base_damage") : Main.playerStats.GetStat("energy_base_damage");
        float multipliedDamage = (stats.GetStat("base_damage") + Main.playerStats.GetStat("base_damage") + typeDamage) * (Main.playerStats.GetStat("damage_mult") + stats.GetStat("damage_mult"));
        return Mathf.RoundToInt(multipliedDamage + Main.playerStats.GetStat("flat_damage"));
    }
}
