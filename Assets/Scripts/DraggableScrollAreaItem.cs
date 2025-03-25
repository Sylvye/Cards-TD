using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DraggableScrollAreaItem : ScrollAreaItem
{
    public enum State
    {
        Home, Moving, Positioned
    }
    [Header("Draggableness")]
    public List<Transform> draggableSnaps = new(); // the locations the item can snap to
    public float snapDist = 1;
    public State state = State.Home;
    private Transform ogParent;

    public override void Start()
    {
        base.Start();
        ogParent = transform.parent;
    }

    private void OnMouseEnter()
    {
        if (state.Equals(State.Home) && Clickable())
        {
            MouseTooltip.SetVisible(true);
            MouseTooltip.SetText(id);
        }
    }

    private void OnMouseExit()
    {
        if (state.Equals(State.Home) && (Clickable() || MouseTooltip.GetText().Equals(id)))
        {
            MouseTooltip.SetVisible(false);
        }
    }

    private void OnMouseDrag()
    {
        SetDestination(Camera.main.ScreenToWorldPoint(Input.mousePosition));
    }

    private void OnMouseUp()
    {
        if (Clickable())
        {
            Transform closest = GetViableSnap(snapDist);
            if (closest != null)
            {
                transform.parent = closest;
                transform.position = closest.position;
                locked = true;
                state = State.Positioned;
            }
            else
            {
                transform.parent = ogParent;
                ScrollAreaInventory ogSA = ogParent.GetComponent<ScrollAreaInventory>();
                ShiftPos(ogSA.scrolledAmt * Vector2.down);
                ogSA.AddToInventory(gameObject, true);
                state = State.Home;
            }
        }
    }

    private void OnMouseDown()
    {
        if (Clickable() && (state.Equals(State.Positioned) || state.Equals(State.Home)))
        {
            locked = false;
            SetDestination(transform.position);
            transform.parent = null;
            state = State.Moving;
            ogParent.GetComponent<ScrollAreaInventory>().RemoveFromInventory(gameObject);
        }
    }

    public override bool Clickable()
    {
        return !state.Equals(State.Home) || (transform.parent.GetComponent<BoxCollider2D>() != null && transform.parent.GetComponent<BoxCollider2D>().OverlapPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition)));
    }

    // returns the destination it can snap to if within range
    private Transform GetViableSnap(float range)
    {
        if (draggableSnaps.Count == 0)
            return null;

        Transform closest = null;
        float dist = float.MaxValue;

        for (int i = 0; i < draggableSnaps.Count; i++)
        {
            Transform o = draggableSnaps[i];
            float d = Vector2.Distance(o.position, GetDestination());
            if (d < dist && o.childCount == 0)
            {
                closest = o;
                dist = d;
            }
        }

        return closest != null && Vector2.Distance(closest.position, transform.position) < range ? closest : null; // if within range, return closest, otherwise return null
    }

    public void SetState(State s)
    {
        state = s;
        locked = s == State.Moving;
    }

    public State GetState()
    {
        return state;
    }
}
