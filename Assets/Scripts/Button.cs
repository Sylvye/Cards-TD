using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class Button : MonoBehaviour
{
    [NonSerialized]
    public Vector3 startScale;
    public Sprite spriteUp;
    public Sprite spriteDown;
    public float scaleAmount = 0.9f;
    [SerializeField]
    private bool active = true; // whether button can be clicked or not

    // Start is called before the first frame update
    public virtual void Start()
    {
        startScale = transform.localScale;
    }

    private void OnMouseEnter()
    {
        if (active)
        {
            transform.localScale = startScale * scaleAmount;
        }
    }

    public void OnMouseExit()
    {
        if (active)
        {
            transform.localScale = startScale;
        }
    }

    private void OnMouseDown()
    {
        if (active && spriteDown != null)
        {
            GetComponent<SpriteRenderer>().sprite = spriteDown;
        }
    }

    private void OnMouseUpAsButton()
    {
        if (active)
        {
            if (spriteUp != null)
                GetComponent<SpriteRenderer>().sprite = spriteUp;
            Action();
        }
    }

    public abstract void Action();

    public void UpdateSprite()
    {
        GetComponent<SpriteRenderer>().sprite = spriteUp;
    }

    public void SetActive(bool a)
    {
        active = a;
        if (a)
        {
            GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
        }
        else
        {
            GetComponent<SpriteRenderer>().color = new Color(0.75f, 0.75f, 0.75f, 0.75f);
        }
    }

    public bool GetActive()
    {
        return active;
    }
}
