using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;
using UnityEngine.UIElements;

public class ScrollAreaItem : MonoBehaviour
{
    public string id;
    [NonSerialized]
    public Vector2 homePos;
    [NonSerialized]
    public SpriteRenderer sr;

    // Start is called before the first frame update
    public virtual void Start()
    {
        homePos = transform.position;
        sr = GetComponent<SpriteRenderer>();
    }

    private void OnMouseEnter()
    {
        if (Clickable())
        {
            MouseTooltip.SetVisible(true);
            MouseTooltip.SetText(id);
        }
    }

    private void OnMouseExit()
    {
        if (Clickable() || MouseTooltip.GetText().Equals(id))
        {
            MouseTooltip.SetVisible(false);
        }
    }

    public virtual bool Clickable()
    {
        return transform.parent.GetComponent<BoxCollider2D>().OverlapPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition));
    }
}
