using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class Card : MonoBehaviour, CardInterface
{
    public static bool isCardSelected = false;
    public string type;
    public GameObject spawnable;
    private GameObject spawned;
    public int tier;
    private bool selected = false;
    private Vector3 handPos;
    private Vector3 lerpPos;
    bool areCardsBeingHovered = false;
    [NonSerialized]
    public Stats stats;

    public virtual void Awake()
    {
        stats = GetComponent<Stats>();
        GetComponent<SpriteRenderer>().sprite = GetSprite();
    }

    public virtual void Start()
    {
        
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

    public virtual GameObject OnPlay()
    {
        return null;
    }

    private void OnMouseDown()
    {
        if (StageController.currentStage == StageController.Stage.Battle && !Spawner.main.IsStageComplete() && areCardsBeingHovered)
        {
            selected = true;
            isCardSelected = true;
            transform.localScale = Vector3.one * 1.5f;
            SetHandPos();
            if (TryGetComponent(out TowerCard tc))
            {
                spawned = tc.OnPlay();
                spawned.layer = 2;
                Main.hitboxReticle_.transform.localScale = 2 * tc.hitboxRadius * Vector3.one;
                Main.towerRangeReticle_.transform.localScale = spawned.GetComponent<Tower>().stats.GetStat("range") * 2 * Vector3.one + Vector3.forward * -6;
            }
        }
    }

    private void OnMouseUp()
    {
        if (!Spawner.main.IsStageComplete())
        {
            selected = false;
            isCardSelected = false;
            if (StageController.currentStage == StageController.Stage.Battle)
            {
                if (TryGetComponent(out TowerCard tc))
                {
                    Main.hitboxReticle_.transform.position = new Vector3(2, 10, 0);
                    Main.towerRangeReticle_.transform.position = new Vector3(4, 10, 0);
                    Main.towerRangeReticle_.transform.localScale = Vector2.one;

                    if (transform.position.y > -2.5 && Physics2D.OverlapCircle(Camera.main.ScreenToWorldPoint(Input.mousePosition), tc.hitboxRadius, Main.placementLayerMask_) == null)
                    {
                        spawned.GetComponent<Tower>().activated = true;
                        spawned.layer = 6;
                        Hand.Remove(this);
                        gameObject.transform.position = Vector3.up * 10;
                        gameObject.transform.localScale = Vector3.one * 1.5f;
                    }
                    else
                    {
                        Destroy(spawned);
                        transform.position = handPos;
                        transform.localScale = Vector3.one * 1.5f;
                    }
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
            if (TryGetComponent(out TowerCard tc))
            {
                Vector3 pos = new Vector3(target.x, target.y, -3);
                if (spawned != null)
                    spawned.transform.position = pos+Vector3.back;
                transform.position = new Vector3(target.x + Main.hitboxReticle_.transform.localScale.x, target.y, -6);
                Main.hitboxReticle_.transform.position = pos;
                Main.towerRangeReticle_.transform.position = pos;
                if (Physics2D.OverlapCircle(target, tc.hitboxRadius, Main.placementLayerMask_) == null)
                    Main.hitboxReticle_.GetComponent<SpriteRenderer>().color = new Color(0.5f, 0.5f, 0.5f, 0.5f);
                else
                    Main.hitboxReticle_.GetComponent<SpriteRenderer>().color = new Color(1, 0, 0, 0.5f);
            }
            else
            {
                transform.position = new Vector3(target.x, target.y, -6);
            }
        }
    }

    public void SetHandPos()
    {
        handPos = (Vector2)transform.parent.position + Hand.GetIndexOf(this) * 2f * Vector2.right;
        handPos.z = -5;
        lerpPos = handPos;
    }

    public virtual string GetName()
    {
        return type;
    }

    public CardInterface FindReference(int index)
    {
        return Cards.GetFromDeck(index);
    }

    public int GetReferenceListLength()
    {
        return Cards.DeckSize();
    }

    public GameObject GetGameObject()
    {
        return gameObject;
    }

    public virtual Sprite GetSprite()
    {
        return GetComponent<SpriteRenderer>().sprite;
    }
}
