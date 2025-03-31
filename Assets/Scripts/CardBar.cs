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
    private Vector3 pos;

    private void Awake()
    {
        main = this;
        state = State.Hidden;
        pos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 l;
        switch (state)
        {
            case State.Hidden:
                transform.position = pos + Vector3.down * 0.7f + Vector3.back * 10;
                break;
            case State.Minimized:
                l = Vector2.Lerp(transform.position, pos, Time.unscaledDeltaTime * lerpSpeed);
                transform.position = new Vector3(l.x, l.y, pos.z);
                break;
            case State.Maximized:
                l = Vector3.Lerp(transform.position, pos + Vector3.up * 3, Time.unscaledDeltaTime * lerpSpeed);
                transform.position = new Vector3(l.x, l.y, pos.z);
                break;
        }
    }

    private void OnMouseOver()
    {
        if (state != State.Hidden)
            state = State.Maximized;
    }

    private void OnMouseExit()
    {
        if (state != State.Hidden) 
            state = State.Minimized;
    }
}
