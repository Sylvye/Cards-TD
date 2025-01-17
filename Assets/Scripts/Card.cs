using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public abstract class Card : MonoBehaviour
{
    public static bool isCardSelected = false;
    public GameObject spawnable;
    public int towerIndex;
    public int tier;
    private bool selected = false;
    private Vector3 handPos;
    public float hitboxRadius;
    private Vector3 lerpPos;
    bool areCardsBeingHovered = false;

    public virtual void Start()
    {
        GetComponent<SpriteRenderer>().sprite = GetSprite();
    }

    private void Update()
    {
        if (StageController.currentStage == StageController.Stage.Battle && Hand.GetIndexOf(this) != -1)
        {
            float mouseY = Camera.main.ScreenToWorldPoint(Input.mousePosition).y;
            float mouseX = Camera.main.ScreenToWorldPoint(Input.mousePosition).x;
            float scale = 1.5f;
            if (mouseY <= -3 && mouseX <= transform.parent.position.x + (Hand.Size()-1) * 2 && !isCardSelected && !areCardsBeingHovered && !Spawner.main.IsStageComplete()) // if mouse enters card bar
            {
                areCardsBeingHovered = true;
                transform.localScale = scale * 2f * Vector3.one;
                lerpPos = handPos + Vector3.up * scale;
            } else if ((mouseY > -3+scale*1.2f || mouseX > transform.parent.position.x + (Hand.Size() - 1) * 2) && areCardsBeingHovered) // if mouse exits card bar
            {
                areCardsBeingHovered = false;
                if (!isCardSelected)
                    transform.localScale = 2 * Vector3.one;
                lerpPos = handPos;
            }

            if (!selected)
            {
                if (Vector2.Distance(transform.position, lerpPos) < 0.01f)
                {
                    transform.position = lerpPos;
                }
                else
                {
                    transform.position = Vector3.Lerp(transform.position, lerpPos, Time.deltaTime * 10f);
                }
            }

            UpdateFX();
        }
    }

    public virtual void UpdateFX()
    {

    }

    public abstract GameObject OnPlay();

    private void OnMouseDown()
    {
        if (StageController.currentStage == StageController.Stage.Battle && !Spawner.main.IsStageComplete() && areCardsBeingHovered)
        {
            selected = true;
            isCardSelected = true;
            transform.localScale = Vector3.one * 1.5f;
            SetHandPos();
            Main.hitboxReticle_.transform.localScale = 2 * hitboxRadius * Vector3.one;
            if (spawnable.TryGetComponent(out Tower t))
            {
                Main.towerRangeReticle_.transform.localScale = GetReticleRadius() * 2 * Vector3.one + Vector3.forward * -6;
            }
        }
    }

    private void OnMouseUp()
    {
        if (!Spawner.main.IsStageComplete())
        {
            selected = false;
            isCardSelected = false;
            Main.hitboxReticle_.transform.position = new Vector3(2, 10, 0);
            if (spawnable.TryGetComponent(out Tower _))
            {
                Main.towerRangeReticle_.transform.position = new Vector3(4, 10, 0);
                Main.towerRangeReticle_.transform.localScale = Vector2.one;
            }
            if (StageController.currentStage == StageController.Stage.Battle)
            {
                if (transform.position.y > -2.5 && Physics2D.OverlapCircle(Camera.main.ScreenToWorldPoint(Input.mousePosition), hitboxRadius, Main.placementLayerMask_) == null)
                {
                    GameObject obj = OnPlay();
                    if (obj != null && obj.TryGetComponent(out Tower t))
                    {
                        t.tier = tier;
                        t.LoadSprite(towerIndex);
                    }
                    Hand.Remove(this);
                    gameObject.transform.position = Vector3.up * 10;
                    gameObject.transform.localScale = Vector3.one * 1.5f;
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
    }

    private void OnMouseDrag()
    {
        if (selected && StageController.currentStage == StageController.Stage.Battle)
        {
            Vector3 target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector3(target.x+Main.hitboxReticle_.transform.localScale.x, target.y, -6);
            Main.hitboxReticle_.transform.position = new Vector3(target.x, target.y, -3);
            if (spawnable.TryGetComponent(out Tower _))
                Main.towerRangeReticle_.transform.position = new Vector3(target.x, target.y, -3);
            if (Physics2D.OverlapCircle(target, hitboxRadius, Main.placementLayerMask_) == null)
                Main.hitboxReticle_.GetComponent<SpriteRenderer>().color = new Color(0.5f, 0.5f, 0.5f, 0.5f);
            else
                Main.hitboxReticle_.GetComponent<SpriteRenderer>().color = new Color(1, 0, 0, 0.5f);
        }
    }

    public abstract float GetReticleRadius();

    public void SetHandPos()
    {
        handPos = (Vector2)transform.parent.position + Hand.GetIndexOf(this) * 2f * Vector2.right;
        handPos.z = -5;
        lerpPos = handPos;
    }

    public Sprite GetSprite()
    {
        return Resources.LoadAll<Sprite>("CardPack")[towerIndex * 5 + tier - 1];
    }
}
