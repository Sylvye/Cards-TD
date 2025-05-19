using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SpriteUIE : MonoBehaviour
{
    private static readonly float LERP_SPEED = 25;

    public string info;
    public bool readInfo;
    public bool locked = false;
    public float zPos;
    public SpriteRenderer sr;
    private Vector2 lerpPos;

    public void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        zPos = transform.position.z;
        OnAwake();
    }

    // Dont delete, messes with inheritance
    public void Start()
    {
        lerpPos = transform.localPosition;
        OnStart();
    }

    // Update is called once per frame
    public void Update()
    {
        if (!locked)
        {
            if (Vector2.Distance(transform.localPosition, lerpPos) > 0.01f)
            {
                transform.localPosition = (Vector3)Vector2.Lerp(transform.localPosition, lerpPos, LERP_SPEED * Time.deltaTime);
            }
            else
            {
                transform.localPosition = GetDestination();
            }

            transform.position = new Vector3(transform.position.x, transform.position.y, zPos);
        }
        OnUpdate();
    }

    public virtual void OnAwake()
    {

    }

    public virtual void OnStart()
    {

    }

    public virtual void OnUpdate()
    {

    }

    public virtual string GetInfo()
    {
        return info;
    }

    public Sprite GetSprite()
    {
        return GetComponent<SpriteRenderer>().sprite;
    }

    public void SetSprite(Sprite s)
    {
        sr.sprite = s;
    }

    /// <summary>
    /// sets the SUIE's lerpPos
    /// </summary>
    /// <param name="dest"></param>
    public virtual void SetDestination(Vector2 dest)
    {
        lerpPos = new Vector2(dest.x, dest.y);
    }

    /// <summary>
    /// returns the SUIE's lerpPos
    /// </summary>
    /// <returns></returns>
    public virtual Vector2 GetDestination()
    {
        return new Vector2(lerpPos.x, lerpPos.y);
    }

    /// <summary>
    /// moves both lerpPos AND transform.position by dir
    /// </summary>
    /// <param name="dir"></param>
    public virtual void ShiftPos(Vector2 dir)
    {
        transform.localPosition = new Vector3(dir.x + transform.localPosition.x, dir.y + transform.localPosition.y, 0);
        transform.position = new Vector3(transform.position.x, transform.position.y, zPos);
        lerpPos = transform.localPosition;
    }
}
