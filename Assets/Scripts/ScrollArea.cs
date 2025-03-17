using System.Collections;
using System.Collections.Generic;
using System.Net;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Progress;
using static UnityEditor.Timeline.Actions.MenuPriority;

public class ScrollArea : MonoBehaviour
{
    public float scrolledAmt = 0;
    public float scrollPower = 0.5f;
    public int layer;

    // Update is called once per frame
    public virtual void Update()
    {
        if (GetComponent<Collider2D>().OverlapPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition))) // handles scrolling
        {
            float scrollAmt = Input.mouseScrollDelta.y * scrollPower;

            float scrolledSum = Mathf.Abs(scrolledAmt + scrollAmt);

            if (scrollAmt != 0 && CanScrollInDir(-scrollAmt))
            {
                scrolledAmt += scrollAmt;
                Vector2 moveAmt = Vector3.up * scrollAmt;
                foreach (Transform child in transform)
                {
                    child.GetComponent<ScrollAreaItem>().ShiftPos(-moveAmt);
                }
            }
        }
    }

    private bool CanScrollInDir(float amount)
    {
        bool up = amount > 0;
        Transform node = up ? transform.GetChild(0) : transform.GetChild(transform.childCount-1);
        Collider2D nodeCollider = node.GetComponent<Collider2D>();
        Collider2D collider = GetComponent<Collider2D>();

        if (up)
        {
            return node.position.y + nodeCollider.bounds.extents.y < transform.position.y + collider.bounds.extents.y;
        }
        else
        {
            return node.position.y - nodeCollider.bounds.extents.y > transform.position.y - collider.bounds.extents.y;
        }
    }
}
