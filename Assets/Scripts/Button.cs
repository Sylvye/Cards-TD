using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class Button : CustomUIElement
{
    public Sprite spriteUp;
    public Sprite spriteDown;
    public Sprite outlineUp;
    public Sprite outlineDown;
    private GameObject outlineObj;
    private SpriteRenderer outlineSR;
    private SpriteRenderer sr;

    public override void Awake()
    {
        base.Awake();
        sr = GetComponent<SpriteRenderer>();
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

    public abstract void Action();

    public virtual void OnMouseEnter()
    {
        if (!Input.GetMouseButtonDown(0))
        {
            MakeSpriteUp();
        }
        if (GetActive())
        {
            if (outlineObj != null)
            {
                ToggleOutline(true);
                outlineSR.sprite = outlineUp;
            }
        }
    }

    public virtual void OnMouseExit()
    {
        if (GetActive())
        {
            if (outlineObj != null)
            {
                ToggleOutline(false);
            }
        }
    }

    public virtual void OnMouseDown()
    {
        if (GetActive() && spriteDown != null)
        {
            GetComponent<SpriteRenderer>().sprite = spriteDown;
            if (outlineObj != null)
            {
                outlineSR.sprite = outlineDown;
            }
        }
    }

    public virtual void OnMouseUpAsButton()
    {
        if (GetActive())
        {
            Action();

            if (spriteUp != null)
                GetComponent<SpriteRenderer>().sprite = spriteUp;
            if (outlineObj != null)
            {
                outlineSR.sprite = outlineUp;
            }
        }
    }

    public void MakeSpriteUp()
    {
        if (spriteUp != null)
            GetComponent<SpriteRenderer>().sprite = spriteUp;
    }

    public void MakeSpriteDown()
    {
        if (spriteUp != null)
            GetComponent<SpriteRenderer>().sprite = spriteDown;
    }

    public void SetSprites(Sprite up, Sprite down)
    {
        spriteUp = up;
        spriteDown = down;
    }

    public override void SetActive(bool a)
    {
        SetActive(a, true);
    }

    public override void SetActive(bool a, bool dim)
    {
        base.SetActive(a, dim);
        if (outlineObj != null && !a)
            ToggleOutline(false);
    }

    public void ToggleOutline(bool a)
    {
        if (outlineObj != null)
            outlineObj.SetActive(a);
    }
}
