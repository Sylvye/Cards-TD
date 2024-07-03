using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Card : MonoBehaviour
{
    private bool selected = false;
    private Vector3 handPos;
    public int indexInHand = -1;
    private Collider2D col;
    public float radius = 1;

    public abstract void OnPlay();

    private void Start()
    {
        col = GetComponent<Collider2D>();
    }

    private void OnMouseDown()
    {
        selected = true;
        transform.localScale = Vector3.one * 0.5f;
        handPos = transform.position;
        Main.hitboxReticle_.transform.localScale = Vector3.one * radius * 2;
    }

    private void OnMouseUp()
    {
        selected = false;
        if (transform.position.y > -2.5 && Physics2D.OverlapCircle(transform.position, radius, Main.placementLayerMask_) == null)
        {
            OnPlay();
            Hand.main.cards.RemoveAt(indexInHand);
            if (Hand.main.cards.Count == 0 )
            {
                Hand.main.Deal();
            }
            Deck.Add(this);
            gameObject.transform.position = Vector3.up * 10;
            gameObject.transform.localScale = Vector3.one * 1.5f;
            Hand.main.DisplayCards();
        }
        else
        {
            transform.position = handPos;
            transform.localScale = Vector3.one * 1.5f;
        }
        Main.hitboxReticle_.transform.position = new Vector3(2, 10, 0);
    }

    private void OnMouseDrag()
    {
        if (selected)
        {
            Vector3 target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector3(target.x, target.y, -7);
            Main.hitboxReticle_.transform.position = new Vector3(target.x, target.y, -4);
            if (Physics2D.OverlapCircle(transform.position, radius, Main.placementLayerMask_) == null)
                Main.hitboxReticle_.GetComponent<SpriteRenderer>().color = new Color(0.5f, 0.5f, 0.5f, 0.5f);
            else
                Main.hitboxReticle_.GetComponent<SpriteRenderer>().color = new Color(1, 0, 0, 0.5f); 
        }
    }
}
