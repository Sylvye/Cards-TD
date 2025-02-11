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
    public Type type = Type.Kinetic;
    public bool activated;

    public virtual void Awake()
    {
        stats = GetComponent<Stats>();
        activated = false;
    }

    public void LoadSprite(int towerIndex)
    {
        GetComponent<SpriteRenderer>().sprite = Resources.LoadAll<Sprite>("TowerArt")[towerIndex * 5 + (int)stats.GetStat("tier") - 1];
    }

    public abstract bool Action();

    public abstract void ApplyTierEffects();

    private void OnMouseEnter()
    {
        if (activated && !Card.isCardSelected && !Spawner.main.IsStageComplete())
        {
            Main.towerRangeReticle_.transform.position = transform.position;
            Main.towerRangeReticle_.transform.localScale = stats.GetStat("range") * 2 * Vector3.one + Vector3.forward * -6;
        }
    }

    private void OnMouseExit()
    {
        if (activated && !Card.isCardSelected)
        {
            Main.towerRangeReticle_.transform.position = new Vector3(4, 10, 0);
            Main.towerRangeReticle_.transform.localScale = Vector2.one;
        }
    }

    public virtual int GetDamage()
    {
        float typeDamage = type == Type.Kinetic ? Main.playerStats.GetStat("kinetic_base_damage") : Main.playerStats.GetStat("energy_base_damage");
        float baseDamage = stats.GetStat("base_damage") + Main.playerStats.GetStat("base_damage") + typeDamage;
        float typeMult = type == Type.Kinetic ? Main.playerStats.GetStat("kinetic_damage_mult") : Main.playerStats.GetStat("energy_damage_mult");
        float damageMult = Main.playerStats.GetStat("damage_mult") * stats.GetStat("damage_mult") * typeMult;
        float multipliedDamage = baseDamage * damageMult;
        int finalDamage = Mathf.RoundToInt(multipliedDamage + Main.playerStats.GetStat("flat_damage"));
        //Debug.Log("Type damage : " + typeDamage + "\nMultiplied base: " + multipliedDamage + "\nFinal: " + finalDamage);
        //Debug.Log("Final Damage: " + finalDamage + "\nKinetic Base: " + Main.playerStats.GetStat("kinetic_base_damage") + "\nEnergy Base: " + Main.playerStats.GetStat("energy_base_damage") + "\nBase Damage: " + stats.GetStat("base_damage") + "\nGlobal Base Damage: " + Main.playerStats.GetStat("base_damage") + "\nGlobal Damage Mult: " + Main.playerStats.GetStat("damage_mult") + "\nDamage Mult: " + stats.GetStat("damage_mult") + "\nFlat Damage: " + Main.playerStats.GetStat("flat_damage"));
        return finalDamage;
    }

    public static Tower MakeTowerByPrefab(GameObject obj, Vector2 pos, Stats s)
    {
        Tower t = Instantiate(obj, new Vector3(pos.x, pos.y, -2), Quaternion.identity).GetComponent<Tower>();
        t.stats.AddStats(s);
        t.ApplyTierEffects();
        return t;
    }
}
