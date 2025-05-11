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
        OnAwake();
    }

    // Dont delete, messes with inheritance
    public void Start()
    {
        lerpPos = transform.position;
        zPos = transform.position.y;
        OnStart();
    }

    // Update is called once per frame
    public void Update()
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
        lerpPos = new Vector3(dest.x, dest.y, zPos);
    }

    /// <summary>
    /// returns the SUIE's lerpPos
    /// </summary>
    /// <returns></returns>
    public virtual Vector3 GetDestination()
    {
        return new Vector3(lerpPos.x, lerpPos.y, zPos);
    }

    /// <summary>
    /// moves both lerpPos AND transform.position by dir
    /// </summary>
    /// <param name="dir"></param>
    public virtual void ShiftPos(Vector2 dir)
    {
        transform.position = new Vector3(dir.x + transform.position.x, dir.y + transform.position.y, zPos);
        lerpPos = transform.position;
    }
}
