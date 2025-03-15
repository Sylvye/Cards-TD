using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class MapController : MonoBehaviour
{
    public static MapController main;
    public static ScrollArea scrollArea;
    public static MapNode currentNode;

    public GameObject nodeObj;
    public string filePath;
    public static GameObject nodeObj_;

    public static Sprite[] sprites;
    public static int nodeX;
    public static int nodeMinibossUp;
    public static int nodeBattleUp;
    public static int nodeShopUp;
    public static int nodeUpgradeUp;
    public static int nodeAugmentUp;
    public static int nodeBossUp;

    private static int[] lengths;
    private static GameObject[] nodes;

    private void Start()
    {
        main = this;
        scrollArea = GetComponent<ScrollArea>();
        nodeObj_ = nodeObj;
        sprites = Resources.LoadAll<Sprite>(filePath);
        
        nodeX = 0;
        nodeMinibossUp = 6;
        nodeBattleUp = 3;
        nodeShopUp = 15;
        nodeUpgradeUp = 9;
        nodeAugmentUp= 12;
        nodeBossUp= 1;

        Screen.SetResolution(2560, 1080, true);
    }

    public static void GenerateMap(int length)
    {
        lengths = new int[length];
        lengths[0] = 1;
        lengths[1] = 2;

        // creates the random array of heights for each column in the map
        for (int i=2; i<length; i++)
        {
            int r = Random.Range(0, 2);

            if (lengths[i - 1] == 3)
                r = Random.Range(-1, 2);
            else if (lengths[i - 1] == 4)
                r = -1;

            lengths[i] = lengths[i - 1] + r;
            if (lengths[i] <= 0)
                lengths[i] = 1;
            else if (lengths[i] >= 5)
                lengths[i] = 4;
        }
        lengths[length - 1] = 1;

        // trims the end of the map to ensure that the lengths will always end with 1
        for (int i=length-1; i>=0; i--)
        {
            if (Mathf.Abs(lengths[i] - lengths[i - 1]) <= 1)
                break;
            else
                lengths[i - 1] = lengths[i] + 1;
        }

        int sum = 0; // sum of numbers in lengths (the total number of nodes to be created)
        foreach (int i in lengths)
            sum += i;

        // fills nodes array with blank nodes
        nodes = new GameObject[sum];
        for (int i=0; i<sum; i++)
        {
            nodes[i] = Instantiate(nodeObj_, Vector3.zero, Quaternion.identity);
            nodes[i].transform.SetParent(main.transform);
            MapNode n = nodes[i].GetComponent<MapNode>();
            n.index = i;
        }

        // positions the nodes
        Vector3 pos = main.transform.position + Vector3.back;
        int index = 0;
        float xOffset = 2.5f;
        float yOffset = 3;
        for (int i=0; i < length; i++)
        {
            pos.x = lengths[i] / -2f * xOffset + xOffset / 2f;
            for (int j=0; j < lengths[i]; j++)
            {
                ScrollAreaItem SAI = nodes[index].GetComponent<ScrollAreaItem>();
                nodes[index].GetComponent<MapNode>().column = i;
                SAI.SetHomePos(pos);
                SAI.zPos = main.transform.position.z-1;
                nodes[index].transform.position = pos;
                pos.x += xOffset;
                index++;
            }
            pos.y += yOffset;
        }

        // assigns nodes their possible exits & sprites
        for (int i=0; i<nodes.Length-1; i++)
        {
            GameObject nObj = nodes[i];
            MapNode n = nObj.GetComponent<MapNode>();

            // assigns sprites
            if (n.column % 2 == 0) // battle stages are every even column
            {
                n.SetSprite(nodeBattleUp);
                n.displayName = "Defense";
                n.stage = StageController.Stage.Battle;
            }
            else
            {
                int r = Random.Range(0, 4);

                switch (r)
                {
                    case 0:
                        n.SetSprite(nodeShopUp);
                        n.displayName = "Shop";
                        n.stage = StageController.Stage.Shop;
                        break;
                    case 1:
                        n.SetSprite(nodeUpgradeUp);
                        n.displayName = "Upgrade";
                        n.stage = StageController.Stage.Upgrade;
                        break;
                    case 2:
                        n.SetSprite(nodeAugmentUp);
                        n.displayName = "Augment";
                        n.stage = StageController.Stage.Augment;
                        break;
                    default:
                        if (Random.Range(0, 1 + n.column/2) == 0)
                        {
                            n.SetSprite(nodeBattleUp);
                            n.displayName = "Defense";
                            n.stage = StageController.Stage.Battle;
                            break;
                        }
                        else
                        {
                            n.SetSprite(nodeMinibossUp);
                            n.displayName = "Miniboss";
                            n.stage = StageController.Stage.Battle;
                            break;
                        }
                }
            }

            n.exits = new MapNode[2];

            // assigns exits
            int exit1Index = i + lengths[n.column];
            int exit2Index = i + lengths[n.column+1];
            if (exit1Index < nodes.Length)
                n.exits[0] = nodes[exit1Index].GetComponent<MapNode>();
            if (exit2Index < nodes.Length)
                n.exits[1] = nodes[exit2Index].GetComponent<MapNode>();

            if (n.exits[0] != null && n.exits[0].column != n.column+1)
                n.exits[0] = null;
            if (n.exits[1] != null && n.exits[1].column != n.column+1)
                n.exits[1] = null;
        }

        MapNode bossNode = nodes[index - 1].GetComponent<MapNode>();
        bossNode.SetSprite(nodeBossUp);
        bossNode.stage = StageController.Stage.Battle;
        bossNode.displayName = "Stage Boss";

        MapNode startNode = nodes[0].GetComponent<MapNode>();

        currentNode = startNode;
    }

    public static void DestroyMap()
    {
        currentNode = null;
        lengths = new int[0];
        foreach (GameObject obj in nodes)
        {
            Destroy(obj);
        }
    }

    // n represents the node in the column to save from being destroyed
    public static void EliminateColumn(MapNode n)
    {
        int col = n.column;
        bool reachedColumn = false;
        foreach (GameObject obj in nodes)
        {
            MapNode objNode = obj.GetComponent<MapNode>();
            if (objNode.column == col)
            {
                reachedColumn = true;
                if (!objNode.Equals(n))
                {
                    objNode.SetSprite(nodeX);
                    objNode.spriteDown = sprites[nodeX];
                    objNode.spriteUp = sprites[nodeX];
                    objNode.stage = StageController.Stage.None;
                    objNode.displayName = "Unreachable";
                    objNode.clickable = false;
                }
            }
            else if (reachedColumn)
                break;
        }
    }

    public static float GetProgress()
    {
        return (float)currentNode.column / (Main.mapLength_ - 1);
    }
}
