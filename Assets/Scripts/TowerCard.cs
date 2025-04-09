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
}
