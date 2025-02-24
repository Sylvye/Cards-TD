using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.UIElements;

public class ScrollAreaItem : MonoBehaviour, IComparable<ScrollAreaItem>
{
    public string id;
    public int order;
    [NonSerialized]
    public Vector2 homePos;
    private SpriteRenderer sr;

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

    public void SetSprite(Sprite s)
    {
        sr.sprite = s;
    }

    public virtual bool Clickable()
    {
        return transform.parent.GetComponent<BoxCollider2D>().OverlapPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition));
    }

    public virtual int CompareTo(ScrollAreaItem other)
    {
        return 10000*(order - other.order) + id.CompareTo(other.id);
    }
}
