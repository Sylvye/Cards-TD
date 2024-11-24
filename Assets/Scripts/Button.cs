using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Button : MonoBehaviour
{
    public Vector3 startScale;
    public Sprite spriteUp;
    public Sprite spriteDown;
    public float scaleAmount = 0.9f;
    [SerializeField]
    private bool active = true; // whether button can be clicked or not

    // Start is called before the first frame update
    void Start()
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
        Debug.Log("Default button method :(");
    }

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
