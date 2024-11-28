using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class ItemDrop : MonoBehaviour
{
    public GameObject item;
    public int tier;

    // Start is called before the first frame update
    private void Start()
    {
        float layer = tier;
        transform.position += Vector3.back * (2.5f + layer * 0.1f);
    }

    private void OnMouseOver()
    {
        GameObject ind = Instantiate(item, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
