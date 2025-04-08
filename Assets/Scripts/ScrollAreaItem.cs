using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.UIElements;

public class ScrollAreaItem : SpriteUIE, IComparable<ScrollAreaItem>
{
    public int sortingOrder;
    [NonSerialized]
    public Vector3 homePos;

    // Start is called before the first frame update
    public override void OnStart()
    {
        homePos = transform.position;
    }

    private void OnMouseEnter()
    {
        if (readInfo && Clickable())
        {
            MouseTooltip.SetVisible(true);
            MouseTooltip.SetText(GetName());
        }
    }

    private void OnMouseExit()
    {
        if ((readInfo && Clickable()) || MouseTooltip.GetText().Equals(info))
        {
            MouseTooltip.SetVisible(false);
        }
    }

    public virtual bool Clickable()
    {
        return transform.parent.GetComponent<BoxCollider2D>().OverlapPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition));
    }

    public virtual int CompareTo(ScrollAreaItem other)
    {
        return 10000*(sortingOrder - other.sortingOrder) + info.CompareTo(other.info);
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
        return info;
    }
}
