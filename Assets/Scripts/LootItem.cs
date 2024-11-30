using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// for items in the loot scroll area. (inventory items)
public abstract class LootItem : ScrollAreaItem
{
    [Header("Loot Item Fields")]
    public bool claimed = false;

    // Start is called before the first frame update
    void Start()
    {
        draggable = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseDown()
    {
        if (!claimed)
        {
            Claim();
            GetComponent<SpriteRenderer>().color = new Color(0.8f, 0.8f, 0.8f, 1);
            //StageController.inventoryLootScrollArea.RemoveFromInventory(gameObject);
            //StageController.inventoryLootScrollArea.RefreshPositions();
            claimed = true;
        }
    }

    public abstract void Claim();
}
