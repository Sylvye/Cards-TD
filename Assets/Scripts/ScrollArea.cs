using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollArea : MonoBehaviour
{
    Vector2 loopingBounds;
    Vector2 offset;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseOver()
    {
        float scrollAmt = Input.mouseScrollDelta.y;

        foreach (Transform child in transform)
        {
            child.transform.position -= Vector3.up * scrollAmt * 0.5f;
        }
    }
}
