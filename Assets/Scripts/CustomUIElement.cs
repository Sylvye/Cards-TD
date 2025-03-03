using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CustomUIElement : MonoBehaviour
{
    private static readonly float LERP_SPEED = 50;
    [SerializeField] private bool active = true;
    public bool locked = false;
    [SerializeField] private Vector2 lerpPos;
    public float zPos;

    public virtual void Awake()
    {
        lerpPos = transform.position;
        zPos = transform.position.y;
    }

    // Start is called before the first frame update
    public virtual void Start()
    {
        
    }

    // Update is called once per frame
    public virtual void Update()
    {
        if (!locked)
        {
            if (Vector2.Distance(transform.position, lerpPos) > 0.01f)
            {
                transform.position = (Vector3)Vector2.Lerp(transform.position, lerpPos, LERP_SPEED * Time.deltaTime) + Vector3.forward * zPos;
            }
            else
            {
                transform.position = GetDestination();
            }
        }
    }

    public virtual Sprite GetSprite()
    {
        return GetComponent<SpriteRenderer>().sprite;
    }

    public virtual void SetActive(bool a)
    {
        active = a;
        if (a)
        {
            GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
        }
        else
        {
            GetComponent<SpriteRenderer>().color = new Color(0.75f, 0.75f, 0.75f, 0.75f);
        }
    }

    public bool GetActive()
    {
        return active;
    }

    public virtual void SetDestination(Vector2 dest)
    {
        lerpPos = new Vector3(dest.x, dest.y, zPos);
    }

    public virtual Vector3 GetDestination()
    {
        return new Vector3(lerpPos.x, lerpPos.y, zPos);
    }

    public virtual void ShiftPos(Vector2 dir)
    {
        transform.position = new Vector3(dir.x + transform.position.x, dir.y + transform.position.y, zPos);
        lerpPos = transform.position;
    }
}
