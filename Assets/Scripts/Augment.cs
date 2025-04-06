using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Augment : MonoBehaviour, CardInterface
{
    public string displayName;
    public string type;
    public string tier;

    [NonSerialized]
    public Stats stats;

    private void Awake()
    {
        stats = GetComponent<Stats>();
    }

    public void ApplyEffect(TowerCard c)
    {
        c.stats.AddStats(stats);
    }

    public string GetName()
    {
        return displayName;
    }

    public string GetTag()
    {
        return displayName;
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
