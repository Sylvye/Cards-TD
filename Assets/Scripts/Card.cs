using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class Card : MonoBehaviour
{
    public GameObject spawnable;
    public int towerIndex;
    public int tier;
    private bool selected = false;
    private Vector3 handPos;
    public float radius;

    private void Start()
    {
        GetComponent<SpriteRenderer>().sprite = Resources.LoadAll<Sprite>("CardPack")[towerIndex * 5 + tier - 1];
    }

    public virtual GameObject OnPlay()
    {
        GameObject obj = Instantiate(spawnable, new Vector3(transform.position.x, transform.position.y, -2), Quaternion.identity);
        return obj;
    }

    private void OnMouseDown()
    {
        if (StageController.stageIndex == 1 && !Spawner.main.IsStageComplete())
        {
            selected = true;
            transform.localScale = Vector3.one * 0.5f;
            SetHandPos();
            Main.hitboxReticle_.transform.localScale = 2 * radius * Vector3.one;
            if (spawnable.TryGetComponent(out Tower t))
            {
                Main.towerRangeReticle_.transform.localScale = t.GetRange(tier) * 2 * Vector3.one + Vector3.forward * -6;
            }
        }
    }

    private void OnMouseUp()
    {
        selected = false;
        Main.hitboxReticle_.transform.position = new Vector3(2, 10, 0);
        if (spawnable.TryGetComponent(out Tower _))
        {
            Main.towerRangeReticle_.transform.position = new Vector3(4, 10, 0);
            Main.towerRangeReticle_.transform.localScale = Vector2.one;
        }
        if (StageController.stageIndex == 1)
        {
            if (transform.position.y > -2.5 && Physics2D.OverlapCircle(transform.position, radius, Main.placementLayerMask_) == null)
            {
                GameObject obj = OnPlay();
                if (obj != null && obj.TryGetComponent(out Tower tower))
                    tower.tier = tier;
                Hand.Remove(Hand.GetIndexOf(this));
                if (Hand.Size() == 0)
                {
                    Hand.Deal();
                }
                Cards.AddToDeck(this);
                gameObject.transform.position = Vector3.up * 10;
                gameObject.transform.localScale = Vector3.one * 1.5f;
                Hand.RepositionHand();
            }
            else
            {
                transform.position = handPos;
                transform.localScale = Vector3.one * 1.5f;
            }
        }
        else
        {
            transform.position = handPos;
            transform.localScale = Vector3.one * 1.5f;
        }
    }

    private void OnMouseDrag()
    {
        if (selected && StageController.stageIndex == 1)
        {
            Vector3 target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector3(target.x, target.y, -6);
            Main.hitboxReticle_.transform.position = new Vector3(target.x, target.y, -3);
            if (spawnable.TryGetComponent(out Tower _))
                Main.towerRangeReticle_.transform.position = new Vector3(target.x, target.y, -3);
            if (Physics2D.OverlapCircle(transform.position, radius, Main.placementLayerMask_) == null)
                Main.hitboxReticle_.GetComponent<SpriteRenderer>().color = new Color(0.5f, 0.5f, 0.5f, 0.5f);
            else
                Main.hitboxReticle_.GetComponent<SpriteRenderer>().color = new Color(1, 0, 0, 0.5f);
        }
    }

    public void SetHandPos()
    {
        handPos = transform.position;
    }
}
