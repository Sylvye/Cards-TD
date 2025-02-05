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


    public virtual void OnMouseEnter()
    {
        if (GetActive())
        {
            transform.localScale = startScale * scaleAmount;
        }
    }

    public virtual void OnMouseExit()
    {
        if (GetActive())
        {
            transform.localScale = startScale;
        }
    }

    public virtual void OnMouseDown()
    {
        if (GetActive() && spriteDown != null)
        {
            GetComponent<SpriteRenderer>().sprite = spriteDown;
        }
    }

    public virtual void OnMouseUpAsButton()
    {
        if (GetActive())
        {
            if (spriteUp != null)
                GetComponent<SpriteRenderer>().sprite = spriteUp;
            Action();
        }
    }

    public void SetSpriteUp()
    {
        if (spriteUp != null)
            GetComponent<SpriteRenderer>().sprite = spriteUp;
    }

    public void SetSpriteDown()
    {
        if (spriteUp != null)
            GetComponent<SpriteRenderer>().sprite = spriteDown;
    }
}
