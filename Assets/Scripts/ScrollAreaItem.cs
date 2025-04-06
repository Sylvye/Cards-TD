using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.UIElements;

public class ScrollAreaItem : CustomUIElement, IComparable<ScrollAreaItem>
{
    public string id;
    public bool readName = true;
    public int order;
    [NonSerialized]
    public Vector3 homePos;
    private SpriteRenderer sr;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        homePos = transform.position;
        sr = GetComponent<SpriteRenderer>();
    }

    private void OnMouseEnter()
    {
        if (readName && Clickable())
        {
            MouseTooltip.SetVisible(true);
            MouseTooltip.SetText(GetName());
        }
    }

    private void OnMouseExit()
    {
        if ((readName && Clickable()) || MouseTooltip.GetText().Equals(id))
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

    public override void ShiftPos(Vector2 dir)
    {
        base.ShiftPos(dir);
        homePos += (Vector3)dir;
    }

    public void SetHomePos()
    {
        SetHomePos(transform.position);
    }

    public void SetHomePos(Vector2 pos)
    {
        homePos = pos;
    }

    public virtual string GetName()
    {
        return id;
    }
}
