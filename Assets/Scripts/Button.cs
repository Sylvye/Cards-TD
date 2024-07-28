using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Button : MonoBehaviour
{
    private Vector3 startScale;
    public Sprite spriteUp;
    public Sprite spriteDown;
    public bool active = true;

    // Start is called before the first frame update
    void Start()
    {
        startScale = transform.localScale;
    }

    private void OnMouseOver()
    {
        if (active)
        {
            transform.localScale = startScale * 0.9f;
        }
    }

    private void OnMouseExit()
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

    public virtual void Action()
    {
        Debug.Log("Clicked");
    }
}
