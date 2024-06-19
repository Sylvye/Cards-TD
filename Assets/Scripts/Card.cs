using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Card : MonoBehaviour
{
    private bool selected = false;
    private Vector3 handPos;
    public int indexInHand = -1;
    private Collider2D col;
    public float radius;
    public GameObject hitboxReticle;

    public abstract void OnPlay();

    private void Start()
    {
        col = GetComponent<Collider2D>();
    }

    private void OnMouseDown()
    {
        if (gameObject.GetComponent<Collider2D>().OverlapPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition)))
        {
            selected = true;
            transform.localScale = Vector3.one;
            handPos = transform.position;
        }
    }

    private void OnMouseUp()
    {
        selected = false;
        if (transform.position.y > -2.5 && Physics2D.OverlapCircle(transform.position, radius, Main.placementLayerMask_) == null)
        {
            OnPlay();
            Hand.main.cards.RemoveAt(indexInHand);
            Deck.Add(this);
            Destroy(gameObject);
            Hand.main.DisplayCards();
        }
        else
        {
            transform.position = handPos;
            transform.localScale = Vector3.one * 1.5f;
        }
    }

    private void OnMouseDrag()
    {
        if (selected)
        {
            Vector3 target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector3(target.x, target.y, 1);
        }
    }
}
