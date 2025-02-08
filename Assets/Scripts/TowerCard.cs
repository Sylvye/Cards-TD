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
        GameObject obj = Instantiate(spawnable, new Vector3(mousePos.x, mousePos.y, -2), Quaternion.identity);
        Tower tower = obj.GetComponent<Tower>();
        stats.AddToStats(tower.stats);
        tower.tier = tier;

        tower.LoadSprite(towerIndex);

        return obj;
    }

    public float GetReticleRadius()
    {
        return prefabTower.CalcRange(tier) + stats.GetStat("range");
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
