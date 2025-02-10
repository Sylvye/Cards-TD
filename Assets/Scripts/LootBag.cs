using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using UnityEngine;

public class LootBag : MonoBehaviour
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
        for (int i=0; i<tier*tier; i++)
        {
            Vector3 dir = new Vector3(Random.Range(-1f, 1), Random.Range(-1f, 1), 0).normalized;
            GameObject ind = Instantiate(item, transform.position, Quaternion.identity);
            ind.GetComponent<Rigidbody2D>().velocity = dir*3;
        }
        Destroy(gameObject);
    }
}
