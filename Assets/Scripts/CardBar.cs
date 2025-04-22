using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardBar : MonoBehaviour
{
    public enum State
    {
        Hidden,
        Minimized,
        Maximized
    }

    public static CardBar main;
    public State state = State.Hidden;
    public float lerpSpeed = 1;
    public Sprite hasComplete;
    public bool forced = false;
    private Sprite hasNoComplete;
    private SpriteRenderer sr;
    private Vector3 pos;

    private void Awake()
    {
        main = this;
        state = State.Hidden;
        pos = transform.position;
        sr = GetComponent<SpriteRenderer>();
        hasNoComplete = sr.sprite;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 l;
        switch (state)
        {
            case State.Hidden:
                transform.position = pos + Vector3.down * 0.7f + Vector3.back * 15;
                forced = false;
                break;
            case State.Minimized:
                l = Vector2.Lerp(transform.position, pos, Time.unscaledDeltaTime * lerpSpeed);
                transform.position = new Vector3(l.x, l.y, pos.z);
                sr.sprite = Hand.CheckForComplete() ? hasComplete : hasNoComplete;
                if (Vector2.Distance(transform.position, pos) < 0.1f)
                {
                    forced = false;
                }
                break;
            case State.Maximized:
                l = Vector3.Lerp(transform.position, pos + Vector3.up * 3, Time.unscaledDeltaTime * lerpSpeed);
                transform.position = new Vector3(l.x, l.y, pos.z);
                sr.sprite = Hand.CheckForComplete() ? hasComplete : hasNoComplete;
                if (Vector2.Distance(transform.position, pos + Vector3.up * 3) < 0.1f)
                {
                    forced = false;
                }
                break;
        }
    }

    public void OnMouseEnter()
    {
        if (state == State.Minimized && !Spawner.main.IsStageCleared() && !forced)
        {
            state = State.Maximized;
            Hand.ReturnAll();
        }
    }

    public void OnMouseExit()
    {
        if (!GetComponent<Collider2D>().OverlapPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition)) && !forced)
        {
            if (state != State.Hidden)
            {
                state = State.Minimized;
            }
        }
    }
}
