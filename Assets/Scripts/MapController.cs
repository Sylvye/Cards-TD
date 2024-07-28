using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class MapController : MonoBehaviour
{
    public static MapController main;
    public static MapNode currentNode;

    public GameObject nodeObj;
    public static GameObject nodeObj_;

    public static Sprite nodeCompleted;
    public static Sprite nodeX;
    public static Sprite nodeCurrentDark;
    public static Sprite nodeCurrentLight;
    public static Sprite nodeMinibossDark;
    public static Sprite nodeMinibossLight;
    public static Sprite nodeBattleDark;
    public static Sprite nodeBattleLight;
    public static Sprite nodeShopDark;
    public static Sprite nodeShopLight;
    public static Sprite nodeUpgradeDark;
    public static Sprite nodeUpgradeLight;
    public static Sprite nodeAugmentDark;
    public static Sprite nodeAugmentLight;
    public static Sprite nodeBossDark;
    public static Sprite nodeBossLight;

    private static int[] lengths;
    private static GameObject[] nodes;

    private void Start()
    {
        main = this;
        Sprite[] sprites = Resources.LoadAll<Sprite>("NodePack");
        nodeObj_ = nodeObj;
        nodeCompleted = sprites[0];
        nodeX = sprites[1];
        nodeCurrentDark = sprites[2];
        nodeCurrentLight = sprites[3];
        nodeMinibossDark = sprites[4];
        nodeMinibossLight = sprites[5];
        nodeBattleDark = sprites[6];
        nodeBattleLight = sprites[7];
        nodeShopDark = sprites[8];
        nodeShopLight = sprites[9];
        nodeUpgradeDark = sprites[10];
        nodeUpgradeLight = sprites[11];
        nodeAugmentDark= sprites[12];
        nodeAugmentLight= sprites[13];
        nodeBossDark= sprites[14];
        nodeBossLight= sprites[15];
    }

    public static void GenerateMap(int length)
    {
        lengths = new int[length];
        lengths[0] = 1;
        lengths[1] = 1;
        lengths[2] = 2;

        // creates the random array of heights for each column in the map
        for (int i=3; i<length; i++)
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
            nodes[i] = Instantiate(nodeObj_, new Vector3(-15, -10, 0), Quaternion.identity);
            nodes[i].transform.SetParent(main.transform);
            MapNode n = nodes[i].GetComponent<MapNode>();
            n.index = i;
        }

        // positions the nodes
        Vector3 pos = new(-10, -10, 0);
        int index = 0;
        for (int i=0; i < length-1; i++)
        {
            pos.y = -9.5f - lengths[i] / 2f;
            for (int j=0; j < lengths[i]; j++)
            {
                nodes[index].GetComponent<MapNode>().column = i;
                nodes[index].transform.position = pos;
                pos.y += 1;
                index++;
            }
            pos.x += 20f/(length-1);
        }
        MapNode bossNode = nodes[index].GetComponent<MapNode>();
        nodes[index].transform.position = new Vector3(10, -10, 0);
        bossNode.column = length-1;
        bossNode.SetSprite(nodeBossDark);
        bossNode.spriteLight = nodeBossLight;
        bossNode.spriteDark = nodeBossDark;
        bossNode.type = "Boss";
        bossNode.displayName = "Stage Boss";

        // assigns nodes their possible exits & sprites
        for (int i=0; i<nodes.Length-1; i++)
        {
            GameObject nObj = nodes[i];
            MapNode n = nObj.GetComponent<MapNode>();

            // assigns sprites
            if (n.column % 2 != 0) // battle stages are every even column
            {
                n.spriteLight = nodeBattleLight;
                n.spriteDark = nodeBattleDark;
                n.SetSprite(nodeBattleDark);
                n.displayName = "Defense";
                n.type = "Defense";
            }
            else
            {
                int r = Random.Range(0, 4);

                switch (r)
                {
                    case 0:
                        n.spriteLight = nodeShopLight;
                        n.spriteDark = nodeShopDark;
                        n.SetSprite(nodeShopDark);
                        n.displayName = "Shop";
                        n.type = "Shop";
                        break;
                    case 1:
                        n.spriteLight = nodeUpgradeLight;
                        n.spriteDark = nodeUpgradeDark;
                        n.SetSprite(nodeUpgradeDark);
                        n.displayName = "Upgrade";
                        n.type = "Upgrade";
                        break;
                    case 2:
                        n.spriteLight = nodeAugmentLight;
                        n.spriteDark = nodeAugmentDark;
                        n.SetSprite(nodeAugmentDark);
                        n.displayName = "Augment";
                        n.type = "Augment";
                        break;
                    default:
                        if (Random.Range(0, 1 + n.column/2) == 0)
                        {
                            n.spriteLight = nodeBattleLight;
                            n.spriteDark = nodeBattleDark;
                            n.SetSprite(nodeBattleDark);
                            n.displayName = "Defense";
                            n.type = "Defense";
                            break;
                        }
                        else
                        {
                            n.spriteLight = nodeMinibossLight;
                            n.spriteDark = nodeMinibossDark;
                            n.SetSprite(nodeMinibossDark);
                            n.displayName = "Miniboss";
                            n.type = "Defense";
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

        MapNode startNode = nodes[0].GetComponent<MapNode>();
        nodes[0].GetComponentInParent<SpriteRenderer>().sprite = nodeCurrentDark;
        startNode.spriteLight = nodeCurrentLight;
        startNode.spriteDark = nodeCurrentDark;
        startNode.type = "Start";
        startNode.displayName = "Start";

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
                    objNode.spriteLight = nodeX;
                    objNode.spriteDark = nodeX;
                }
            }
            else if (reachedColumn)
                break;
        }
    }
}
