using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;
using UnityEngine.UIElements;

public class ScrollAreaItem : MonoBehaviour
{
    public List<GameObject> destinations = new List<GameObject>();
    public float snapDist = 0.5f;
    public Vector2 lerpPos;
    Vector2 ogScale;
    Vector2 scale;
    SpriteRenderer sr;
    public Vector3 pos;
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
        Vector2 finalLerpPos = lerpPos;
        if (home)
        {
            finalLerpPos += Vector2.up * transform.GetComponentInParent<ScrollArea>().scrolledAmt;
            finalLerpPos.x = pos.x;
            sr.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
            Debug.Log(finalLerpPos.x);
        }
        if ((Vector2)transform.position != finalLerpPos)
        {
            Debug.Log(finalLerpPos.x);
            if (home)
            {
                transform.position.y = Vector3.Slerp(transform.position.y, finalLerpPos.y, Time.deltaTime * 20) + Vector3.back * 2;
            }
            else
            {
                transform.position = Vector3.Slerp((Vector2)transform.position, finalLerpPos, Time.deltaTime * 20) + Vector3.back * 2;
            }
            //Debug.Log(transform.position.x +", "+ finalLerpPos.x);
            if (Vector2.Distance(transform.position, finalLerpPos) < 0.02f)
            {
                transform.position = (Vector3)finalLerpPos + Vector3.back * 2;
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
