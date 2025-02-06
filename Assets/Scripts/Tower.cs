using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Tower : MonoBehaviour
{
    public enum Type
    {
        Kinetic,
        Energy
    }
    public Stats stats = new();
    public int tier = 1;
    public Type type = Type.Kinetic;
    public int damage;
    public float attackSpeed;
    public float damageMultiplier;
    public float range;
    public float explosionRadius;

    public void LoadSprite(int towerIndex)
    {
        GetComponent<SpriteRenderer>().sprite = Resources.LoadAll<Sprite>("TowerArt")[towerIndex * 5 + tier - 1];
    }

    public virtual float GetRange(int t)
    {
        return range;
    }

    public abstract void InitTierEffects();

    private void OnMouseEnter()
    {
        if (!Card.isCardSelected && !Spawner.main.IsStageComplete())
        {
            Main.towerRangeReticle_.transform.position = transform.position;
            Main.towerRangeReticle_.transform.localScale = range * 2 * Vector3.one + Vector3.forward * -6;
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
        float multipliedDamage = (damage + Main.playerStats.GetStat("base_damage") + typeDamage) * Main.playerStats.GetStat("mult_damage") * damageMultiplier;
        return Mathf.RoundToInt(multipliedDamage + Main.playerStats.GetStat("flat_damage"));
    }
}
