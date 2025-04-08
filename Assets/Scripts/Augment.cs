using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Augment : Puppetable
{
    public string displayName;
    public string info;
    public string type;
    public Dictionary<string, Stat> stats;
    
    public Augment()
    {
        displayName = "";
        info = "";
        type = "";
        stats = new();
    }

    public Augment(string displayName, string info, string type, Dictionary<string, Stat> s)
    {
        this.displayName = displayName;
        this.info = info;
        this.type = type;
        stats = s;
    }

    public void ApplyEffect(TowerCard c)
    {
        c.stats;
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
        return GetComponent<SpriteRenderer>().sprite;
    }

    public GameObject GetGameObject()
    {
        return gameObject;
    }
}
