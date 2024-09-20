using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class ItemDrop : MonoBehaviour
{
    public string itemName;
    public string rarity;
    public int amount = 1;

    // Start is called before the first frame update
    void Start()
    {
        float layer = RarityToNum(rarity);
        transform.position -= Vector3.forward * layer * 0.5f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int RarityToNum(string r)
    {
        switch (r)
        {
            case "Common":      return 0;
            case "Rare":        return 1;
            case "Unique":      return 2;
            case "Epic":        return 2;
            case "Legendary":   return 3;
            default:            return -1;
        }
    }
}
