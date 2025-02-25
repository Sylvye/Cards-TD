using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class Button : CustomUIElement
{
    public float scaleAmount = 0.9f;
    public Sprite spriteUp;
    public Sprite spriteDown;
    public Sprite outlineUp;
    public Sprite outlineDown;
    private GameObject outlineObj;
    private SpriteRenderer outlineSR;

    public virtual void Awake()
    {
        if (outlineUp != null && outlineDown != null)
        {
            outlineObj = new GameObject("outline");
            outlineObj.transform.parent = transform;
            outlineObj.transform.localPosition = Vector3.back;
            outlineSR = outlineObj.AddComponent<SpriteRenderer>();
            outlineSR.sprite = outlineUp;
            outlineObj.SetActive(false);
        }
    }

    public virtual void OnMouseEnter()
    {
        if (!Input.GetMouseButtonDown(0))
        {
            MakeSpriteUp();
        }
        if (GetActive())
        {
            transform.localScale = startScale * scaleAmount;
            if (outlineObj != null)
            {
                outlineObj.SetActive(true);
                outlineSR.sprite = outlineUp;
            }
        }
    }

    public virtual void OnMouseExit()
    {
        if (GetActive())
        {
            transform.localScale = startScale;
            if (outlineObj != null)
            {
                outlineObj.SetActive(false);
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
}
