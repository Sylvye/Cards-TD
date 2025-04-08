using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public abstract class Button : SpriteUIE
{
    public Sprite spriteUp;
    public Sprite spriteDown;
    public Sprite outlineUp;
    public Sprite outlineDown;
    private GameObject outlineObj;
    private SpriteRenderer outlineSR;
    private bool clickable;

    public override void OnAwake()
    {
        if (outlineUp != null && outlineDown != null)
        {
            outlineObj = new GameObject("outline");
            outlineObj.transform.parent = transform;
            outlineObj.transform.localPosition = Vector3.back;
            outlineSR = outlineObj.AddComponent<SpriteRenderer>();
            outlineSR.sprite = outlineUp;
            outlineSR.sortingOrder = sr.sortingOrder;
            outlineSR.sortingLayerID = sr.sortingLayerID;
            outlineSR.sortingLayerName = sr.sortingLayerName;
            outlineSR.maskInteraction = sr.maskInteraction;
            ToggleOutline(false);
        }
    }

    public virtual void OnMouseEnter()
    {
        if (!Input.GetMouseButtonDown(0))
        {
            UseSpriteUp();
        }
        if (outlineObj != null && IsClickable())
        {
            ToggleOutline(true);
            outlineSR.sprite = outlineUp;
        }
    }

    public virtual void OnMouseExit()
    {
        if (outlineObj != null)
        {
            ToggleOutline(false);
        }
    }

    public virtual void OnMouseDown()
    {
        if (IsClickable() && spriteDown != null)
        {
            UseSpriteDown();
            if (outlineObj != null)
            {
                outlineSR.sprite = outlineDown;
            }
        }
    }

    public virtual void OnMouseUpAsButton()
    {
        if (IsClickable())
        {
            Action();

            if (spriteUp != null)
                UseSpriteUp();
            if (outlineObj != null)
            {
                outlineSR.sprite = outlineUp;
            }
        }
    }

    public abstract void Action();

    public void SetClickable(bool c)
    {
        clickable = c;
        if (outlineObj != null && !c)
            ToggleOutline(false);
    }

    public virtual bool IsClickable()
    {
        return clickable;
    }

    public void UseSpriteUp()
    {
        if (spriteUp != null)
            UseSpriteUp();
    }

    public void UseSpriteDown()
    {
        if (spriteUp != null)
            UseSpriteDown();
    }

    public void SetSprites(Sprite up, Sprite down)
    {
        spriteUp = up;
        spriteDown = down;
    }

    public void ToggleOutline(bool a)
    {
        if (outlineObj != null)
            outlineObj.SetActive(a);
    }
}
