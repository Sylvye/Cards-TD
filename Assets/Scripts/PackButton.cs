using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PackButton : Button
{
    public int packIndex;
    public GameObject lootObj;

    private void Update()
    {
        if (Main.main.packs[packIndex] > 0)
        {
            SetActive(true);
        }
        else
        {
            SetActive(false);
        }
    }

    public override void Action()
    {
        int packs = Main.main.packs[packIndex];
        if (packs > 0)
        {
            Main.main.packs[packIndex]--;
            GameObject inventoryObj = Instantiate(lootObj);
            StageController.inventoryLootScrollArea.AddToInventory(inventoryObj);
        }
        Main.UpdatePackLabels();
    }
}
