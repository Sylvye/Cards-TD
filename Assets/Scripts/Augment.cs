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

    public void ApplyEffect(TowerCard c)
    {
        c.range += range;
        c.flatDamage += flatDamage;
        c.attackSpeed += attackSpeed;
        c.spread -= spread;
        c.homingSpeed += homingSpeed;
        c.projectiles += projectiles;
        c.projectileSpeedMult += projectileSpeedMult;
        c.pierce += pierce;
        c.explosionRadius += explosionRadius;
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
