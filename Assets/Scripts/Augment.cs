using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Augment : MonoBehaviour, CardInterface
{
    public string type;

    [NonSerialized]
    public Stats stats;

    private void Awake()
    {
        stats = GetComponent<Stats>();
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
