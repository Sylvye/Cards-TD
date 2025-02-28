using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using static UnityEditor.Progress;
using static UnityEditor.Timeline.Actions.MenuPriority;

public class ScrollArea : MonoBehaviour
{
    public int itemsPerRow;
    public Vector2 offset;
    public Vector3 startPos;
    public float scrolledAmt = 0;
    public float scrollPower = 0.5f;
    public int layer;
    private List<ScrollAreaItem> inventory = new();

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
                    child.GetComponent<ScrollAreaItem>().ShiftPos(-moveAmt);
                }
            }
        }
    }
    
    public void DeleteInventory()
    {
        foreach (ScrollAreaItem child in inventory)
        {
            Destroy(child.gameObject);
        }
        inventory.Clear();
    }

    public void AddToInventory(GameObject item)
    {
        AddToInventory(item, false);
    }

    public void AddToInventory(GameObject item, bool refresh)
    {
        inventory.Add(item.GetComponent<ScrollAreaItem>());
        item.transform.parent = transform;
        item.GetComponent<SpriteRenderer>().maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
        if (refresh)
            RefreshPositions();
    }

    public void AddToInventory(GameObject[] items)
    {
        AddToInventory(items, false);
    }

    public void AddToInventory(GameObject[] items, bool refresh)
    {
        foreach (GameObject item in items)
        {
            inventory.Add(item.GetComponent<ScrollAreaItem>());
            item.transform.parent = transform;
            item.GetComponent<SpriteRenderer>().maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
        }
        if (refresh)
            RefreshPositions();
    }

    public void RemoveFromInventory(GameObject item)
    {
        RemoveFromInventory(item, false);
    }

    public void RemoveFromInventory(GameObject item, bool refresh)
    {
        inventory.Remove(item.GetComponent<ScrollAreaItem>());
        item.GetComponent<SpriteRenderer>().maskInteraction = SpriteMaskInteraction.None;
        if (refresh)
            RefreshPositions();
    }

    public void ClearClaimed()
    {
        for (int i=inventory.Count-1; i>=0; i--)
        {
            GameObject obj = inventory[i].gameObject;
            if (obj.TryGetComponent(out LootItem li) && li.claimed)
            {
                Destroy(obj);
                inventory.RemoveAt(i);
            }
        }
    }

    public void FillWithCards(GameObject prefabObj, Transform destination, int sortingOrder, Cards.CardType type)
    {
        var numCards = type switch
        {
            Cards.CardType.Card => Cards.DeckSize(),
            Cards.CardType.Augment => Cards.AugmentSize(),
            _ => 0,
        };


        for (int i = 0; i < numCards; i++) // places cards in deck scroll area
        {
            GameObject itemObj = Instantiate(prefabObj, Vector3.one, Quaternion.identity);
            AddToInventory(itemObj);

            SpriteRenderer sr = itemObj.GetComponent<SpriteRenderer>();
            CardInterface cardPrefabReference = type switch
            {
                Cards.CardType.Card => Cards.GetFromDeck(i),
                Cards.CardType.Augment => Cards.GetFromAugments(i),
                _ => null,
            };
            sr.sortingOrder = sortingOrder;
            sr.sprite = cardPrefabReference.GetSprite();

            ScrollAreaItemCard item = itemObj.GetComponent<ScrollAreaItemCard>();
            item.draggableSnaps.Add(destination);
            item.prefabReference = cardPrefabReference.GetGameObject();
            item.id = cardPrefabReference.GetName();
        }
        RefreshPositions();
    }

    public void FillWithList(List<ScrollAreaItem> list, Transform destination, int sortingOrder)
    {
        for (int i = 0; i < list.Count; i++) // places items in deck scroll area
        {
            GameObject prefab = list[i].gameObject;
            GameObject itemObj = Instantiate(prefab, Vector3.one, Quaternion.identity);
            AddToInventory(itemObj);

            SpriteRenderer sr = itemObj.GetComponent<SpriteRenderer>();
            ScrollAreaItem sai = itemObj.GetComponent<ScrollAreaItem>();
            sr.sortingOrder = sortingOrder;

            if (itemObj.TryGetComponent(out DraggableScrollAreaItem dsai))
            {
                dsai.draggableSnaps.Add(destination);
            }
        }
        RefreshPositions();
    }

    private void RefreshPositions()
    {
        inventory.Sort();
        for (int i = 0; i < inventory.Count; i++)
        {
            CustomUIElement item = inventory[i].GetComponent<CustomUIElement>();
            //item.SetDestination(new Vector3(offset.x * (i % itemsPerRow) / transform.localScale.x, -offset.y * (i / itemsPerRow) / transform.localScale.y) + startPos + Vector3.down * scrolledAmt + Vector3.back); // localPos, original solution
            //item.SetDestination(transform.position + new Vector3(offset.x * (i % itemsPerRow) / transform.localScale.x, -offset.y * (i / itemsPerRow) / transform.localScale.y, -1) + startPos + Vector3.down * scrolledAmt);
            item.SetDestination(transform.position + new Vector3(offset.x * (i % itemsPerRow), -offset.y * (i / itemsPerRow)) + new Vector3(startPos.x*transform.localScale.x, startPos.y*transform.localScale.y) + Vector3.down * scrolledAmt);
            item.zPos = transform.position.z - 1;
        }
    }
}
