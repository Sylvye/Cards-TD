using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ScrollAreaItem : MonoBehaviour
{
    public List<GameObject> destinations = new List<GameObject>();
    public float snapDist = 0.5f;
    Vector2 lerpPos;
    Vector2 ogScale;
    Vector2 scale;
    SpriteRenderer sr;
    Vector3 pos;
    Transform p;
    bool home = true;

    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        pos = transform.position;
        scale = transform.localScale;
        ogScale = scale;
        p = transform.parent;
        lerpPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if ((Vector2)transform.position != lerpPos)
        {
            transform.position = Vector3.Slerp((Vector2)transform.position, lerpPos, Time.deltaTime * 20) + Vector3.back*2;
            if (Vector2.Distance(transform.position, lerpPos) < 0.02f)
            {
                transform.position = (Vector3)lerpPos + Vector3.back * 2;
                if (home)
                {
                    sr.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
                }
            }
        }
    }

    private void OnMouseEnter()
    {
        if (!Input.GetMouseButton(0))
            transform.localScale = scale * 1.1f;
    }

    private void OnMouseExit()
    {
        if (!Input.GetMouseButton(0))
            transform.localScale = scale;
    }

    private void OnMouseDrag()
    {
        lerpPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    private void OnMouseUp()
    {
        transform.localScale = scale;

        GameObject closest = GetViableDestination(snapDist);
        if (closest != null)
        {
            transform.parent = closest.transform;
            transform.localScale = closest.transform.localScale;
            lerpPos = closest.transform.position + Vector3.back;
            scale = transform.localScale;
        }
        else
        {
            lerpPos = pos;
            transform.parent = p;
            scale = ogScale;
            transform.localScale = scale;
            home = true;
        }
    }

    private void OnMouseDown()
    {
        transform.parent = null;
        home = false;
        sr.maskInteraction = SpriteMaskInteraction.None;
    }

    // returns the destination it can snap to if within range
    private GameObject GetViableDestination(float range)
    {
        if (destinations.Count == 0) 
            return null;

        GameObject closest = null;
        float dist = float.MaxValue;

        for (int i=0; i<destinations.Count; i++)
        {
            GameObject o = destinations[i];
            float d = Vector2.Distance(o.transform.position, lerpPos);
            if (d < dist && o.transform.childCount == 0)
            {
                closest = o;
                dist = d;
            }
        }

        return closest != null && Vector2.Distance(closest.transform.position, transform.position) < range ? closest : null; // if within range, return closest, otherwise return null
    }
}
