using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class ItemDrop : MonoBehaviour
{
    public string itemName;
    public string category;
    public int tier;
    public float fadeAmt = 10f;
    SpriteRenderer sr;

    // Start is called before the first frame update
    void Start()
    {
        float layer = tier;
        transform.position -= Vector3.forward * layer * 0.5f;
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnMouseOver()
    {
        StartCoroutine(PickupEffect());
    }

    public int CategoryToNum(string r)
    {
        return r switch
        {
            "Fighter" => 0,
            "Hoarder" => 1,
            "Artisan" => 2,
            _ => -1,
        };
    }

    IEnumerator PickupEffect()
    {
        for (int i=0; i<20; i++)
        {
            sr.enabled = false;
            yield return new WaitForSeconds(0.05f);
            transform.position += Vector3.up * 0.01f;
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, sr.color.a - fadeAmt * Time.deltaTime);
            sr.enabled = true;
        }
        Destroy(gameObject);
    }
}
