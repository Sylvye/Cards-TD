using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CustomUIElement : MonoBehaviour
{
    [NonSerialized]
    public Vector3 startScale;
    [SerializeField]
    private bool active = true;

    // Start is called before the first frame update
    public virtual void Start()
    {
        startScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual Sprite GetSprite()
    {
        return GetComponent<SpriteRenderer>().sprite;
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

    public abstract void Action();
}
