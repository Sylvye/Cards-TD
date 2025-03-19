using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// for items in the loot scroll area. (inventory items)
public abstract class LootItem : ScrollAreaItem
{
    private void OnMouseOver()
    {
        if (TryGetComponent(out AnimateMaterial am))
        {
            am.Activate();
        }
    }

    public abstract void Claim();
}
