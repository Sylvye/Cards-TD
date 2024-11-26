using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PackButton : Button
{
    public int packIndex;
    GameObject obj;

    public override void Action()
    {
        int packs = Main.main.packs[packIndex];
        if (packs > 0)
        {
            Main.main.packs[packIndex]--;
            GameObject inventoryObj = Instantiate(obj);
            StageController.inventoryLootScrollArea.AddToInventory(inventoryObj);
        }
    }
}
