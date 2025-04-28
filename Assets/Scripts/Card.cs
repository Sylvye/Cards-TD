using AYellowpaper.SerializedCollections;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public abstract class Card : Puppetable, ICloneable
{
    private Sprite sprite;
    public string type;
    public float cooldown;
    [SerializedDictionary("Name", "Stat")]
    public Stats stats;

    public Card()
    {
        type = "";
        cooldown = 0;
        stats = null;
        sprite = null;
    }

    public Card(string type, float cooldown, Stats stats, Sprite sprite)
    {
        this.type = type;
        this.cooldown = cooldown;
        this.stats = stats;
        this.sprite = sprite;
    }

    public virtual GameObject OnPlay()
    {
        return null;
    }

    /// <summary>
    /// returns the "title" of a card (name + tier)
    /// </summary>
    /// <returns></returns>
    public string GetName()
    {
        return type + " t" + stats["tier"];
    }

    public virtual string GetInfo()
    {
        return type;
    }

    public Sprite GetSprite()
    {
        return sprite;
    }

    public void SetSprite(Sprite s)
    {
        sprite = s;
    }

    public virtual Sprite CalcSprite(int tier)
    {
        return sprite;
    }

    public virtual void UpdateSprite()
    {
        sprite = CalcSprite((int)stats["tier"].value);
    }

    public abstract object Clone();
}
