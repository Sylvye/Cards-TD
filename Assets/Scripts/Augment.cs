using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Augment : MonoBehaviour, CardInterface
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
    public float explosionRadius;

    public Stats stats;

    private void Awake()
    {
        Debug.Log(name + " Awakened");
        stats = new();
        stats.AddStat("flatDamage", flatDamage);
        stats.AddStat("attack_speed", attackSpeed);
        stats.AddStat("range", range);
        stats.AddStat("pierce", pierce);
        stats.AddStat("projectiles", projectiles);
        stats.AddStat("mult_speed", projectileSpeedMult);
        stats.AddStat("homing", homingSpeed);
        stats.AddStat("spread", spread);
        stats.AddStat("explosion_radius", explosionRadius);
    }

    public void ApplyEffect(TowerCard c)
    {
        stats.AddToStats(c.stats);
    }

    public string GetName()
    {
        return type;
    }

    public Sprite GetSprite()
    {
        return GetComponent<SpriteRenderer>().sprite;
    }

    public GameObject GetGameObject()
    {
        return gameObject;
    }

    public CardInterface FindReference(int index)
    {
        return Cards.GetFromAugments(index);
    }

    public int GetReferenceListLength()
    {
        return Cards.AugmentSize();
    }
}
