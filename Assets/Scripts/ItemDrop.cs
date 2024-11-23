using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class ItemDrop : MonoBehaviour
{
    public GameObject indicator;
    public int tier;

    // Start is called before the first frame update
    void Start()
    {
        float layer = tier;
        transform.position -= Vector3.forward * layer * 0.5f;
    }

    private void OnMouseOver()
    {
        GameObject ind = Instantiate(indicator, transform.position, Quaternion.identity);
        indicator.GetComponent<Rigidbody2D>().AddForce(Vector2.up*20);
        Destroy(gameObject);
    }
}
