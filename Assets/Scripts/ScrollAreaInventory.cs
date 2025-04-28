using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollAreaInventory : ScrollArea
{
    public int itemsPerRow;
    public Vector2 offset;
    public Vector3 startPos;
    private List<ScrollAreaItem> inventory = new();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void DeleteInventory()
    {
        foreach (ScrollAreaItem child in inventory)
        {
            Destroy(child.gameObject);
        }
        inventory.Clear();
        scrolledAmt = 0;
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
            Puppetable puppetable = type switch
            {
                Cards.CardType.Card => Cards.GetFromDeck(i),
                Cards.CardType.Augment => Cards.GetFromAugments(i),
                _ => null,
            };
            sr.sortingOrder = sortingOrder;
            sr.sprite = puppetable.GetSprite();

            ScrollAreaItemCard item = itemObj.GetComponent<ScrollAreaItemCard>();
            item.draggableSnaps.Add(destination);
            item.reference = Puppet.MakePuppet();
            item.info = puppetable.GetInfo();
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
            SpriteUIE item = inventory[i].GetComponent<SpriteUIE>();
            item.SetDestination(transform.position + new Vector3(offset.x * (i % itemsPerRow), -offset.y * (i / itemsPerRow)) + new Vector3(startPos.x * transform.localScale.x, startPos.y * transform.localScale.y) + Vector3.down * scrolledAmt);
            item.zPos = transform.position.z - 1;
        }
    }
}
