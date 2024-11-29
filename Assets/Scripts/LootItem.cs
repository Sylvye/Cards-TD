using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// for items in the loot scroll area. (inventory items)
public abstract class LootItem : ScrollAreaItem
{
    public Sprite sprite;


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
        Claim();
        StageController.inventoryLootScrollArea.RemoveFromInventory(gameObject);
        StageController.inventoryLootScrollArea.RefreshPositions();
    }

    public abstract void Claim();
}
