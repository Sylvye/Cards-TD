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
        if (TryGetComponent(out DraggableScrollAreaItem sai))
        {
            item.transform.localScale = sai.ogScale;
        }
    }

    public void ClearClaimed()
    {
        for (int i=inventory.Count-1; i>=0; i--)
        {
            GameObject obj = inventory[i];
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
                Cards.CardType.Augment => Cards.GetFromDeck(i),
                _ => null,
            };
            sr.sortingOrder = sortingOrder;
            sr.sprite = cardPrefabReference.GetSprite();

            ScrollAreaItemCard item = itemObj.GetComponent<ScrollAreaItemCard>();
            item.draggableDestinations.Add(destination);
            item.prefabReference = cardPrefabReference.GetGameObject();
            item.id = cardPrefabReference.GetName();
        }
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
                dsai.draggableDestinations.Add(destination);
            }
        }
    }
}
