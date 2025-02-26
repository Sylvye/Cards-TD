using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class TowerCard : Card
{
    private Tower prefabTower;
    public GameObject spawnable;
    public GameObject spawned;
    public int towerIndex;
    public float hitboxRadius;

    public override void Awake()
    {
        base.Awake();
        prefabTower = spawnable.GetComponent<Tower>();
    }

    public override void MouseUpAction()
    {
        Main.towerHitboxReticle_.transform.position = new Vector3(2, 10, 0);
        Main.towerRangeReticle_.transform.position = new Vector3(4, 10, 0);
        Main.towerRangeReticle_.transform.localScale = Vector2.one;

        if (transform.position.y > -2.5 && Physics2D.OverlapCircle(Camera.main.ScreenToWorldPoint(Input.mousePosition), hitboxRadius, Main.placementLayerMask_) == null)
        {
            spawned.GetComponent<Tower>().activated = true;
            spawned.layer = 6;
            Hand.Remove(this);
            gameObject.transform.position = Vector3.up * 10;
        }
        else
        {
            ReturnToHand();
        }
    }

    public override void MouseDownAction()
    {
        if (TryGetComponent(out TowerCard tc))
        {
            spawned = tc.OnPlay();
            spawned.layer = 2;
            Main.towerHitboxReticle_.transform.localScale = 2 * tc.hitboxRadius * Vector3.one;
            Main.towerRangeReticle_.transform.localScale = spawned.GetComponent<Tower>().stats.GetStat("range") * 2 * Vector3.one + Vector3.forward * -6;
        }
    }

    public override void MouseDragAction(Vector3 target)
    {
        Vector3 pos = new(target.x, target.y, 3);
        if (spawned != null)
            spawned.transform.position = pos + Vector3.back;
        transform.position = new Vector3(target.x + Main.towerHitboxReticle_.transform.localScale.x, target.y, -6);
        Main.towerHitboxReticle_.transform.position = pos;
        Main.towerRangeReticle_.transform.position = pos;
        if (Physics2D.OverlapCircle(target, hitboxRadius, Main.placementLayerMask_) == null)
            Main.towerHitboxReticle_.GetComponent<SpriteRenderer>().color = new Color(0.5f, 0.5f, 0.5f, 0.5f);
        else
            Main.towerHitboxReticle_.GetComponent<SpriteRenderer>().color = new Color(1, 0, 0, 0.5f);
    }

    public override GameObject OnPlay()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Tower t = Tower.MakeTowerByPrefab(spawnable, mousePos, stats);
        t.LoadSprite(towerIndex);

        return t.gameObject;
    }

    public override void ReturnToHand()
    {
        base.ReturnToHand();
        if (spawned != null)
            Destroy(spawned);
    }

    public override string GetName()
    {
        if (stats == null)
        {
            stats = GetComponent<Stats>();
        }
        return prefabTower.name + " T" + stats.GetStat("tier");
    }

    public override Sprite GetSprite()
    {
        if (stats == null)
        {
            stats = GetComponent<Stats>();
        }
        return Resources.LoadAll<Sprite>("CardPack")[towerIndex * 5 + (int)stats.GetStat("tier") - 1];
    }

    public override Sprite GetSprite(int tier)
    {
        return Resources.LoadAll<Sprite>("CardPack")[towerIndex * 5 + tier - 1];
    }
}
