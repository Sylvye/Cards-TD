using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// for items in the loot scroll area. (inventory items)
public abstract class LootItem : ScrollAreaItem
{
    [Header("Loot Item Fields")]
    public bool claimed = false;

    private void OnMouseDown()
    {
        if (Clickable() && !claimed)
        {
            Claim();
            GetComponent<SpriteRenderer>().color = new Color(0.8f, 0.8f, 0.8f, 1);
            claimed = true;
        }
    }

    public abstract void Claim();
}
