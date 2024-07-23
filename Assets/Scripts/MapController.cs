using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class MapController : MonoBehaviour
{
    public static MapNode currentNode;

    public GameObject node;
    public static GameObject node_;

    public Sprite nodeDark;
    public static Sprite nodeDark_;

    public Sprite nodeLight;
    public static Sprite nodeLight_;

    public Sprite nodeMainDark;
    public static Sprite nodeMainDark_;

    public Sprite nodeMainLight;
    public static Sprite nodeMainLight_;

    public Sprite nodeX;
    public static Sprite nodeX_;

    public Sprite nodeBossDark;
    public static Sprite nodeBossDark_;

    public Sprite nodeBossLight;
    public static Sprite nodeBossLight_;

    private static int[] lengths;
    private static GameObject[] nodes;

    private void Start()
    {
        node_ = node;
        nodeDark_ = nodeDark;
        nodeLight_ = nodeLight;
        nodeMainDark_ = nodeMainDark;
        nodeMainLight_ = nodeMainLight;
        nodeX_ = nodeX;
        nodeBossDark_ = nodeBossDark;
        nodeBossLight_ = nodeBossLight;
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
            nodes[i] = Instantiate(node_, new Vector3(-15, -9, 0), Quaternion.identity);
            nodes[i].GetComponent<MapNode>().index = i;
        }

        // positions the nodes
        Vector3 pos = new(-10, -9, 0);
        int index = 0;
        for (int i=0; i < length-1; i++)
        {
            pos.y = -8.5f - lengths[i] / 2f;
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
        nodes[index].transform.position = new Vector3(10, -9, 0);
        bossNode.column = length-1;
        bossNode.SetSprite(nodeBossDark_);
        bossNode.spriteLight = nodeBossLight_;
        bossNode.spriteDark = nodeBossDark_;

        // assigns nodes their possible exits
        for (int i=0; i<nodes.Length-1; i++)
        {
            GameObject nObj = nodes[i];
            MapNode n = nObj.GetComponent<MapNode>();
            n.exits = new MapNode[2];

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
        nodes[0].GetComponentInParent<SpriteRenderer>().sprite = nodeMainDark_;
        startNode.spriteLight = nodeMainLight_;
        startNode.spriteDark = nodeMainDark_;

        currentNode = startNode;
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
                    objNode.SetSprite(nodeX_);
                    objNode.spriteLight = nodeX_;
                    objNode.spriteDark = nodeX_;
                }
            }
            else if (reachedColumn)
                break;
        }
    }
}
