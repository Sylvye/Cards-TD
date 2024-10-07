using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;
using UnityEngine.UIElements;

public class ScrollAreaItem : MonoBehaviour
{
    public enum State
    {
        Home, Moving, Positioned
    }
    public List<GameObject> destinations = new List<GameObject>();
    public float snapDist = 1;
    public Vector2 lerpPos;
    public Vector2 homePos;
    public State s = State.Home;
    Transform p;
    public Vector2 ogScale;
    Vector2 scale;
    SpriteRenderer sr;
    

    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        scale = transform.localScale;
        ogScale = scale;
        p = transform.parent;
        lerpPos = transform.position;
        homePos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos;
        switch (s)
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
        if (!Input.GetMouseButton(0))
            transform.localScale = scale * 1.1f;
    }

    private void OnMouseExit()
    {
        if (!Input.GetMouseButton(0))
            transform.localScale = scale;
    }

    private void OnMouseDrag()
    {
        lerpPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    private void OnMouseUp()
    {
        transform.localScale = scale;

        GameObject closest = GetViableDestination(snapDist);
        if (closest != null)
        {
            transform.parent = closest.transform;
            transform.localScale = closest.transform.localScale;
            transform.position = closest.transform.position + Vector3.back;
            scale = transform.localScale;
            s = State.Positioned;
        }
        else
        {
            transform.parent = p;
            scale = ogScale;
            transform.localScale = scale;
            homePos += p.GetComponent<ScrollArea>().scrolledAmt * Vector2.down;
            p.GetComponent<ScrollArea>().AddToInventory(gameObject);
            s = State.Home;
            sr.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
        }
    }

    private void OnMouseDown()
    {
        if (s.Equals(State.Positioned) || s.Equals(State.Home))
        {
            lerpPos = transform.position;
            transform.parent = null;
            sr.maskInteraction = SpriteMaskInteraction.None;
            s = State.Moving;
            p.GetComponent<ScrollArea>().RemoveFromInventory(gameObject);
            p.GetComponent<ScrollArea>().RefreshPositions();
        }
    }

    // returns the destination it can snap to if within range
    private GameObject GetViableDestination(float range)
    {
        if (destinations.Count == 0) 
            return null;

        GameObject closest = null;
        float dist = float.MaxValue;

        for (int i=0; i<destinations.Count; i++)
        {
            GameObject o = destinations[i];
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
