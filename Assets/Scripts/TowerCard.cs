using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using static UnityEngine.RuleTile.TilingRuleOutput;

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

    public TowerCard(string type, float cooldown, GameObject towerObj, float hitboxRadius, int towerIndex, Stats stats, Sprite sprite) : base(type, cooldown, stats, sprite)
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

    public override object Clone()
    {
        return new TowerCard();
    }
}
