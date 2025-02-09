using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TowerCard : Card
{
    private Tower prefabTower;
    public int towerIndex;
    public float hitboxRadius;

    public override void Awake()
    {
        base.Awake();
        prefabTower = spawnable.GetComponent<Tower>();
    }

    public override GameObject OnPlay()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Tower t = Tower.MakeTowerByPrefab(spawnable, mousePos, stats);
        t.LoadSprite(towerIndex);

        return t.gameObject;
    }

    public override string GetName()
    {
        return prefabTower.name + " T" + tier;
    }

    public override Sprite GetSprite()
    {
        return Resources.LoadAll<Sprite>("CardPack")[towerIndex * 5 + tier - 1];
    }
}
