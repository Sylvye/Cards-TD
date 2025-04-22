using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEditor;
using UnityEngine;
using UnityEngine.U2D;

public abstract class CardPuppet : Puppet
{

    public static Transform battlefield;

    private Card card;

    public Stats stats;
    private Vector3 handPos;
    protected bool selected;

    public override void OnAwake()
    {
        base.OnAwake();
        battlefield = GameObject.Find("Field").transform;
        stats = card.stats; // Temp
        SetSprite(card.GetSprite());
        stats = GetComponent<Stats>();
    }

    public override void OnUpdate()
    {
        if (!selected)
        {
            ma.Set("_UnscaledTime", Time.unscaledTime);
            ma.Set(Mathf.Clamp((Time.time - Hand.timeOfLastPlay) / stats.GetStat("cooldown"), 0, 1));
        }
    }

    public virtual void OnMouseUp()
    {
        if (selected && Hand.GetIndexOf(card) != -1) // if card is in hand
        {
            selected = false;
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

    public virtual void OnMouseDrag()
    {
        if (selected)
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
            selected = true;
        }
    }

    /// <summary>
    /// When the player presses down on a card, selecting it
    /// </summary>
    public virtual void MouseDownAction()
    {

    }

    /// <summary>
    /// When the player releases the card, playing it
    /// </summary>
    /// <returns></returns>
    public abstract bool MouseUpAction();

    public virtual void MouseDragAction(Vector3 target)
    {
        SetDestination(new Vector3(target.x, target.y, -6));
    }

    public virtual void ReturnToHand()
    {
        transform.parent = Hand.main.transform;
        transform.localPosition = handPos;
        SetDestination(handPos);
    }

    public void SetHandPos()
    {
        handPos = Hand.GetIndexOf(card) * 1.5f * Vector2.right;
        handPos.z = -5;
    }

    public static void ClearField()
    {
        foreach (Transform child in battlefield)
        {
            Destroy(child.gameObject);
        }
    }

    public bool IsOffCooldown()
    {
        return Time.time - Hand.timeOfLastPlay >= stats.GetStat("cooldown");
    }
}
