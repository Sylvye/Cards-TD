using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public abstract class Card : MonoBehaviour, CardInterface
{
    public string type;
    private Vector3 handPos;
    public float cooldown;
    [NonSerialized]
    public Stats stats;
    public static Transform playingField;
    private bool clicked;
    private MaterialAnimator ma;

    public virtual void Awake()
    {
        playingField = GameObject.Find("Field").transform;
        stats = GetComponent<Stats>();
        ma = GetComponent<MaterialAnimator>();
        GetComponent<SpriteRenderer>().sprite = GetSprite();
        clicked = false;
    }

    // dont delete, method is overriden
    public virtual void Start()
    {
        
    }

    public virtual void Update()
    {
        if (!clicked)
        {
            ma.Set("_UnscaledTime", Time.unscaledTime);
            ma.Set(Mathf.Clamp((Time.time - Hand.timeOfLastPlay) / cooldown, 0, 1));
        }
    }

    public virtual GameObject OnPlay()
    {
        return null;
    }

    private void OnMouseDown()
    {
        if (IsOffCooldown())
        {
            SetHandPos();
            transform.parent = transform.parent.parent;

            MouseDownAction();

            CardBar.main.state = CardBar.State.Minimized;
            CardBar.main.forced = true;
            StageController.ToggleTime(true);
            clicked = true;
        }
    }

    private void OnMouseUp()
    {
        if (clicked && Hand.GetIndexOf(this) != -1) // if card is in hand
        {
            clicked = false;
            // stage isnt cleared & stage == battle & the mouse isnt over the card bar
            if (!Spawner.main.IsStageCleared() && StageController.currentStage == StageController.Stage.Battle && !CardBar.main.GetComponent<Collider2D>().OverlapPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition)))
            {
                if (MouseUpAction())
                {
                    Hand.timeOfLastPlay = Time.time;
                    if (BattleButton.phase == 0)
                    {
                        BattleButton.main.Action();
                    } 
                }
                else // failed, wrong card specific / overlap conditions
                {
                    ReturnToHand();
                }
            }
            else // failed, wrong global conditions
            {
                ReturnToHand();
                CardBar.main.forced = false;
                CardBar.main.state = CardBar.State.Maximized;
            }
        }
    }

    private void OnMouseDrag()
    {
        if (clicked)
        {
            if (StageController.currentStage == StageController.Stage.Battle)
            {
                MouseDragAction(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            }
            else
            {
                ReturnToHand();
            }
        }
    }

    public abstract void MouseDownAction();

    // true if successful, false if not
    public abstract bool MouseUpAction();

    public virtual void MouseDragAction(Vector3 target)
    {
        transform.position = new Vector3(target.x, target.y, -6);
    }

    public virtual void ReturnToHand()
    {
        transform.parent = Hand.main.transform;
        transform.localPosition = handPos;
    }

    public void SetHandPos()
    {
        handPos = Hand.GetIndexOf(this) * 1.5f * Vector2.right;
        handPos.z = -5;
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
        return GetSprite(1);
    }

    public virtual Sprite GetSprite(int tier)
    {
        return GetComponent<SpriteRenderer>().sprite;
    }

    public void UpdateSprite()
    {
        GetComponent<SpriteRenderer>().sprite = GetSprite((int)stats.GetStat("tier"));
    }

    public static void ClearField()
    {
        foreach (Transform child in playingField)
        {
            Destroy(child.gameObject);
        }
    }

    public bool IsOffCooldown()
    {
        return Time.time - Hand.timeOfLastPlay >= cooldown;
    }
}
