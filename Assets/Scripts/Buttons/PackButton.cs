using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PackButton : Button
{
    public ItemLootpool itemLootpool;
    public int packIndex;
    public GameObject lootObj;

    public override void Update()
    {
        base.Update();
        if (Main.main.packs[packIndex] > 0)
        {
            SetActive(true);
        }
        else
        {
            SetActive(false);
        }
    }

    public GameObject CreateItem()
    {
        GameObject inventoryObj = Instantiate(itemLootpool.GetRandom().gameObject);

        return inventoryObj;
    }

    public override void Action()
    {
        int packs = Main.main.packs[packIndex];
        int loops = 1;
        if (Input.GetKey(KeyCode.LeftShift)) // if you hold shift while you click the button, it will open all packs instantly
        {
            loops = packs;
        }

        for (int i = 0; i < loops; i++)
        {
            if (packs > 0)
            {
                Main.main.packs[packIndex]--;
                StageController.inventoryLootScrollArea.AddToInventory(CreateItem(), true);
            }
        }
        Main.UpdatePackLabels();
    }
}
