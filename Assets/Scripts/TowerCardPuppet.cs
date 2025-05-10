using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerCardPuppet : CardPuppet
{
    public GameObject spawned;
    public float hitboxRadius;
    protected TowerCard tc;

    public override void OnStart()
    {
        base.OnStart();
        tc = (TowerCard)card;
        hitboxRadius = tc.hitboxRadius;
    }

    public override bool MouseUpAction()
    {
        Main.main.towerHitboxReticle.transform.position = new Vector3(2, 10, 0);
        Main.main.towerRangeReticle.transform.position = new Vector3(4, 10, 0);
        Main.main.towerRangeReticle.transform.localScale = Vector2.one;

        if (transform.position.y > -2.5 && Physics2D.OverlapCircle(Camera.main.ScreenToWorldPoint(Input.mousePosition), hitboxRadius, Main.main.placementLayerMask) == null)
        {
            spawned.GetComponent<Tower>().activated = true;
            spawned.layer = 6;
            Hand.Remove(card);
            gameObject.transform.position = Vector3.up * 10;
            return true;
        }
        else
        {
            ReturnToHand();
            return false;
        }
    }

    public override void MouseDownAction()
    {
        spawned = tc.OnPlay();
        spawned.layer = 2;
        Main.main.towerHitboxReticle.transform.localScale = 2 * hitboxRadius * Vector3.one;
        Main.main.towerRangeReticle.transform.localScale = spawned.GetComponent<Tower>().stats.GetStat("range") * 2 * Vector3.one + Vector3.forward * -6;
    }

    public override void MouseDragAction(Vector3 target)
    {
        Vector3 pos = new(target.x, target.y, 3);
        if (spawned != null)
            spawned.transform.position = pos + Vector3.back;
        transform.position = new Vector3(target.x + Main.main.towerHitboxReticle.transform.localScale.x, target.y, -6);
        Main.main.towerHitboxReticle.transform.position = pos;
        Main.main.towerRangeReticle.transform.position = pos;
        if (Physics2D.OverlapCircle(target, hitboxRadius, Main.main.placementLayerMask) == null)
            Main.main.towerHitboxReticle.GetComponent<SpriteRenderer>().color = new Color(0.5f, 0.5f, 0.5f, 0.5f); 

        else
            Main.main.towerHitboxReticle.GetComponent<SpriteRenderer>().color = new Color(1, 0, 0, 0.5f); // TEMP
    }

    public override void ReturnToHand()
    {
        base.ReturnToHand();
        Main.main.towerHitboxReticle.transform.position = new Vector3(2, 10, 0);
        Main.main.towerRangeReticle.transform.position = new Vector3(4, 10, 0);
        Main.main.towerRangeReticle.transform.localScale = Vector2.one;
        if (spawned != null)
            Destroy(spawned);
    }
}
