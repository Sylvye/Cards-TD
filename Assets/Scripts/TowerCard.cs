using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TowerCard : Card
{
    public GameObject towerObj;
    public float hitboxRadius;
    public int towerIndex;

    public TowerCard() : base()
    {
        towerObj = null;
        hitboxRadius = 1;
        towerIndex = 0;
    }

    public TowerCard(string type, GameObject towerObj, float hitboxRadius, int towerIndex, Stats stats) : base(type, stats, CalcSprite((int)stats.GetStat("tier"), towerIndex))
    {
        this.towerObj = towerObj;
        this.hitboxRadius = hitboxRadius;
        this.towerIndex = towerIndex;
    }

    public override GameObject OnPlay()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Tower t = Tower.MakeTowerByPrefab(towerObj, mousePos, stats);
        t.LoadSprite(towerIndex);

        return t.gameObject;
    }

    public override Sprite CalcSprite(int tier)
    {
        return Resources.LoadAll<Sprite>("CardPack")[towerIndex * 5 + tier - 1];
    }
    
    public static Sprite CalcSprite(int tier, int towerIndex)
    {
        int index = towerIndex * 5 + tier - 1;
        if (index < 0)
            index = 0;
        return Resources.LoadAll<Sprite>("CardPack")[index];
    }

    public override object Clone()
    {
        return new TowerCard(type, Object.Instantiate(towerObj, towerObj.transform.position, Quaternion.identity), hitboxRadius, towerIndex, (Stats)stats.Clone());
    }
}
