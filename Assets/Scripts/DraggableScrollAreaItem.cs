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
    public List<GameObject> draggableDestinations = new();
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

            GameObject closest = GetViableDestination(snapDist);
            if (closest != null)
            {
                transform.parent = closest.transform;
                transform.localScale = closest.transform.localScale;
                transform.position = closest.transform.position + Vector3.back;
                scale = transform.localScale;
                state = State.Positioned;
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
            p.GetComponent<ScrollArea>().RefreshPositions();
        }
    }

    public override bool Clickable()
    {
        return !state.Equals(State.Home) || (transform.parent.GetComponent<BoxCollider2D>() != null && transform.parent.GetComponent<BoxCollider2D>().OverlapPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition)));
    }

    // returns the destination it can snap to if within range
    private GameObject GetViableDestination(float range)
    {
        if (draggableDestinations.Count == 0)
            return null;

        GameObject closest = null;
        float dist = float.MaxValue;

        for (int i = 0; i < draggableDestinations.Count; i++)
        {
            GameObject o = draggableDestinations[i];
            float d = Vector2.Distance(o.transform.position, lerpPos);
            if (d < dist && o.transform.childCount == 0)
            {
                closest = o;
                dist = d;
            }
        }

        return closest != null && Vector2.Distance(closest.transform.position, transform.position) < range ? closest : null; // if within range, return closest, otherwise return null
    }
}
