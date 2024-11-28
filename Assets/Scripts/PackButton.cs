using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PackButton : Button
{
    public SpriteLootpool[] spriteLootpools;
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

    public GameObject CreateItem()
    {
        GameObject inventoryObj = Instantiate(lootObj);
        inventoryObj.GetComponent<SpriteRenderer>().sprite = spriteLootpools[packIndex].GetRandom();

        // make items add their value here

        return inventoryObj;
    }

    public override void Action()
    {
        int packs = Main.main.packs[packIndex];
        if (packs > 0)
        {
            Main.main.packs[packIndex]--;
            StageController.inventoryLootScrollArea.AddToInventory(CreateItem());
            StageController.inventoryLootScrollArea.RefreshPositions();
        }
        Main.UpdatePackLabels();
    }
}
