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
    public List<Transform> draggableDestinations = new();
    public float snapDist = 1;
    public State state = State.Home;
    private Transform p;
    private Vector2 lerpPos;
    [NonSerialized]
    public Vector2 ogScale;
    private Vector2 scale;

    public override void Start()
    {
        base.Start();

        lerpPos = transform.position;
        scale = transform.localScale;
        ogScale = scale;
        p = transform.parent;
    }

    private void Update()
    {
        Vector3 pos;
        switch (state)
        {
            case State.Home:
                break;
            case State.Moving:
                pos = Vector3.Lerp(transform.position, lerpPos, Time.deltaTime * 50);
                transform.position = new Vector3(pos.x, pos.y, -2);
                break;
            case State.Positioned:
                break;
        }
    }

    private void OnMouseEnter()
    {
        if (state.Equals(State.Home) && Clickable())
        {
            if (!Input.GetMouseButton(0))
                transform.localScale = scale * 1.1f;
            MouseTooltip.SetVisible(true);
            MouseTooltip.SetText(id);
        }
    }

    private void OnMouseExit()
    {
        if (!Input.GetMouseButton(0))
            transform.localScale = scale;
        if (state.Equals(State.Home) && (Clickable() || MouseTooltip.GetText().Equals(id)))
        {
            MouseTooltip.SetVisible(false);
        }
    }

    private void OnMouseDrag()
    {
        lerpPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    private void OnMouseUp()
    {
        if (Clickable())
        {
            transform.localScale = scale;

            Transform closest = GetViableDestination(snapDist);
            if (closest != null)
            {
                transform.parent = closest;
                transform.localScale = closest.localScale;
                transform.localPosition = Vector3.zero;
                scale = transform.localScale;
                state = State.Positioned;
                p.GetComponent<ScrollArea>().RefreshPositions();
            }
            else
            {
                transform.parent = p;
                scale = ogScale;
                transform.localScale = scale;
                homePos += p.GetComponent<ScrollArea>().scrolledAmt * Vector2.down;
                p.GetComponent<ScrollArea>().AddToInventory(gameObject);
                state = State.Home;
            }
        }
    }

    private void OnMouseDown()
    {
        if (Clickable() && (state.Equals(State.Positioned) || state.Equals(State.Home)))
        {
            lerpPos = transform.position;
            transform.parent = null;
            state = State.Moving;
            p.GetComponent<ScrollArea>().RemoveFromInventory(gameObject);
            //p.GetComponent<ScrollArea>().RefreshPositions();
        }
    }

    public override bool Clickable()
    {
        return !state.Equals(State.Home) || (transform.parent.GetComponent<BoxCollider2D>() != null && transform.parent.GetComponent<BoxCollider2D>().OverlapPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition)));
    }

    // returns the destination it can snap to if within range
    private Transform GetViableDestination(float range)
    {
        if (draggableDestinations.Count == 0)
            return null;

        Transform closest = null;
        float dist = float.MaxValue;

        for (int i = 0; i < draggableDestinations.Count; i++)
        {
            Transform o = draggableDestinations[i];
            float d = Vector2.Distance(o.position, lerpPos);
            if (d < dist && o.childCount == 0)
            {
                closest = o;
                dist = d;
            }
        }

        return closest != null && Vector2.Distance(closest.position, transform.position) < range ? closest : null; // if within range, return closest, otherwise return null
    }
}
