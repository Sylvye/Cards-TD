using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour
{
    public GameObject node;
    public static GameObject node_;

    private static int[] lengths;
    private static GameObject[] nodes;

    private void Start()
    {
        node_ = node;
        GenerateMap(10);
    }

    public static void GenerateMap(int length)
    {
        lengths = new int[length];
        lengths[0] = 1;
        
        // creates the random array of heights for each column in the map
        for (int i=1; i<length; i++)
        { 
            lengths[i] = lengths[i - 1] + Random.Range(0, 2);
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
            nodes[i] = Instantiate(node_, new Vector3(-15, -10, 0), Quaternion.identity);
            nodes[i].GetComponent<MapNode>().index = i;
        }

        // positions the nodes
        Vector3 pos = new Vector3(-10, -10, 0);
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
        nodes[index].transform.position = new Vector3(10, -10, 0);
        nodes[index].GetComponent<MapNode>().column = length-1;

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
    }
}
