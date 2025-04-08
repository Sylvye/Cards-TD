using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// for items in the loot scroll area. (inventory items)
public abstract class LootItem : ScrollAreaItem
{
    public override void OnStart()
    {
        if (TryGetComponent(out MaterialAnimator am))
            am.Set("_seed", Random.Range(-100, 100));
    }

    private void OnMouseOver()
    {
        if (TryGetComponent(out MaterialAnimator am))
        {
            am.Activate();
        }
    }

    public abstract void Claim();
}
