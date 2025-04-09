using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.U2D;

public abstract class CardPuppet : SpriteUIE
{
    public static Transform battlefield;

    public Puppetable reference;
    public Card card;
    public float cooldown;
    public Stats stats;
    private Vector3 handPos;
    private MaterialAnimator ma;
    private bool clicked;

    public override void OnAwake()
    {
        battlefield = GameObject.Find("Field").transform;
        ma = GetComponent<MaterialAnimator>();
        stats = GetComponent<Stats>();
        stats.SetStatsFromDict(card.stats); // Temp
        SetSprite(card.GetSprite());
        cooldown = card.cooldown;
    }

    // Update is called once per frame
    public override void OnUpdate()
    {
        if (!clicked)
        {
            ma.Set("_UnscaledTime", Time.unscaledTime);
            ma.Set(Mathf.Clamp((Time.time - Hand.timeOfLastPlay) / cooldown, 0, 1));
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
            clicked = true;
        }
    }

    private void OnMouseUp()
    {
        if (clicked && Hand.GetIndexOf(card) != -1) // if card is in hand
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
        return Time.time - Hand.timeOfLastPlay >= cooldown;
    }
}
