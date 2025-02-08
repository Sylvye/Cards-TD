using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class Tower : MonoBehaviour
{
    // checked!
    public enum Type
    {
        Kinetic,
        Energy
    }
    [NonSerialized]
    public Stats stats;
    public int tier;
    public Type type = Type.Kinetic;

    public virtual void Awake()
    {
        stats = GetComponent<Stats>();
    }

    public void LoadSprite(int towerIndex)
    {
        GetComponent<SpriteRenderer>().sprite = Resources.LoadAll<Sprite>("TowerArt")[towerIndex * 5 + tier - 1];
    }

    public float GetRange()
    {
        return CalcRange(tier);
    }

    public virtual float CalcRange(int t)
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
        int finalDamage = Mathf.RoundToInt(multipliedDamage + Main.playerStats.GetStat("flat_damage"));
        Debug.Log("Type damage : " + typeDamage + "\nMultiplied base: " + multipliedDamage + "\nFinal: " + finalDamage);
        return finalDamage;
    }
}
