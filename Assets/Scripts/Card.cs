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
    protected Sprite sprite;
    public string type;
    [SerializedDictionary("Name", "Stat")]
    public Stats stats;

    public Card()
    {
        type = "";
        stats = null;
        sprite = null;
        stats = new();
    }

    public Card(string type, Stats stats, Sprite sprite)
    {
        this.type = type;
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

    public Puppet MakePuppet()
    {
        GameObject p = new GameObject();
        p.AddComponent<SpriteRenderer>();
        p.AddComponent<MaterialAnimator>();
        p.name = "Card Puppet";
        TowerCardPuppet tcp = p.AddComponent<TowerCardPuppet>();
        tcp.SetReference(this);
        //Debug.Log("Set reference");
        return tcp;
    }

    public abstract object Clone();
}
