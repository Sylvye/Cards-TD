using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class ScrollArea : MonoBehaviour
{
    public int itemsPerRow;
    public Vector2 offset;
    public Vector3 startPos;
    public float scrolledAmt = 0;
    int itemCount = 0;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (GetComponent<Collider2D>().OverlapPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition)))
        {
            int scrollAmt = (int)Input.mouseScrollDelta.y;

            if (scrollAmt != 0)
            {
                scrolledAmt += scrollAmt;
                foreach (Transform child in transform)
                {
                    child.transform.position -= Vector3.up * scrollAmt * 0.5f;
                }
            }
        }
    }
    
    public void AddToInventory(GameObject item)
    {
        Vector2 scale = item.transform.localScale;
        item.transform.parent = transform;
        item.transform.localPosition = new Vector3(offset.x * (itemCount % itemsPerRow) / transform.localScale.x, -offset.y * (itemCount++ / itemsPerRow) / transform.localScale.y, -1) + startPos;
        item.transform.localScale = scale / transform.localScale;
    }
}
