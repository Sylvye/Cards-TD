using AYellowpaper.SerializedCollections;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Augment : Puppetable, ICloneable
{
    public Sprite sprite;
    public string displayName;
    public string info;
    public string type;
    [SerializedDictionary("Name", "Stat")]
    public Stats stats;
    
    public Augment()
    {
        displayName = "";
        info = "";
        type = "";
        stats = new();
    }

    public Augment(string displayName, string info, string type, Stats s)
    {
        this.displayName = displayName;
        this.info = info;
        this.type = type;
        stats = s;
    }

    public void ApplyEffect(TowerCard c)
    {
        c.stats.AddStats(stats);
    }

    public string GetName()
    {
        return displayName;
    }

    public string GetInfo()
    {
        return info;
    }

    public Sprite GetSprite()
    {
        return sprite;
    }

    public object Clone()
    {
        return new Augment(displayName, info, type, (Stats)stats.Clone());
    }
}
