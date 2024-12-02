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
    public float scrollPower = 0.5f;
    public int layer;
    public bool hovered = false;
    private List<GameObject> inventory = new List<GameObject>();

    // Start is called before the first frame update
    private void Start()
    {

    }

    // Update is called once per frame
    private void Update()
    {
        if (GetComponent<Collider2D>().OverlapPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition))) // handles scrolling
        {
            float scrollAmt = Input.mouseScrollDelta.y * scrollPower;

            float scrollDest = Mathf.Abs(scrolledAmt + scrollAmt);

            if (scrollAmt != 0 && scrollDest < inventory.Count / itemsPerRow * offset.y/transform.localScale.y)
            {
                scrolledAmt += scrollAmt;
                foreach (Transform child in transform)
                {
                    Vector2 moveAmt = Vector3.up * scrollAmt;
                    child.transform.localPosition -= (Vector3)moveAmt;
                    child.GetComponent<ScrollAreaItem>().homePos -= moveAmt;
                }
            }
        }
    }
    
    public void ClearInventory()
    {
        inventory.Clear();
    }

    public void RefreshPositions()
    {
        for (int i=0; i<inventory.Count; i++)
        {
            GameObject item = inventory[i];
            item.transform.localPosition = new Vector3(offset.x * (i % itemsPerRow) / transform.localScale.x, -offset.y * (i / itemsPerRow) / transform.localScale.y, -1) + startPos + Vector3.down * scrolledAmt;
        }
    }

    public void AddToInventory(GameObject item)
    {
        inventory.Add(item);
        item.transform.parent = transform;
        int itemIndex = inventory.Count - 1;
        item.transform.localPosition = new Vector3(offset.x * (itemIndex % itemsPerRow) / transform.localScale.x, -offset.y * (itemIndex++ / itemsPerRow) / transform.localScale.y, -1) + startPos + Vector3.down * scrolledAmt;
        item.GetComponent<SpriteRenderer>().maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
    }

    public void AddToInventory(GameObject[] items)
    {
        foreach (GameObject item in items)
        {
            AddToInventory(item);
        }
    }

    public void RemoveFromInventory(GameObject item)
    {
        inventory.Remove(item);
        item.GetComponent<SpriteRenderer>().maskInteraction = SpriteMaskInteraction.None;
        if (TryGetComponent(out ScrollAreaItem sai))
        {
            item.transform.localScale = sai.ogScale;
        }
    }

    private void OnMouseEnter()
    {
        hovered = true;
    }
    private void OnMouseExit()
    {
        hovered = false;
    }
}
