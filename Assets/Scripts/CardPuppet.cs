using AYellowpaper.SerializedCollections;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEditor;
using UnityEngine;
using UnityEngine.U2D;

public abstract class CardPuppet : SpriteUIE, Puppet
{
    protected Puppetable reference;

    protected Card card;
    protected bool selected;
    protected Vector3 lastPos;

    private Stats stats;
    private MaterialAnimator ma;

    public override void OnAwake()
    {
        base.OnAwake();
        ma = GetComponent<MaterialAnimator>();
        lastPos = transform.position;
    }

    public override void OnStart()
    {
        //Debug.Log("STARTED, card is null: "+(card==null));
        base.OnStart();
        SetSprite(card.GetSprite());
        stats = card.stats;
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
                    Return();
                }
            }
            else // failed, wrong global conditions
            {
                Return();
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
                Return();
            }
        }
    }

    private void OnMouseDown()
    {
        if (IsOffCooldown())
        {
            lastPos = transform.position;
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

    public bool IsOffCooldown()
    {
        return Time.time - Hand.timeOfLastPlay >= stats.GetStat("cooldown");
    }

    public Puppetable GetReference()
    {
        return reference;
    }

    public void SetReference(Puppetable r)
    {
        reference = r;
        card = (Card)reference;
        SetSprite(r.GetSprite());
    }

    public virtual void Return()
    {
        Debug.Log(lastPos);
        transform.position = lastPos;
        SetDestination(transform.localPosition);
    }
}
