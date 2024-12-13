using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectableScrollAreaItem : ScrollAreaItem
{
    public bool selected = false;

    private void OnMouseDown()
    {
        if (Clickable())
        {
            transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = !transform.GetChild(0).GetComponent<SpriteRenderer>().enabled;
            selected = !selected;
        }
    }
}
