using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Tower : MonoBehaviour
{
    public int tier = 1;
    public float damageMultiplier;
    public float range;

    public virtual float GetRange(int t)
    {
        return range;
    }

    private void OnMouseEnter()
    {
        Main.towerRangeReticle_.transform.position = transform.position;
        Main.towerRangeReticle_.transform.localScale = range * 2 * Vector3.one + Vector3.forward * -6;
    }

    private void OnMouseExit()
    {
        Main.towerRangeReticle_.transform.position = new Vector3(4, 10, 0);
        Main.towerRangeReticle_.transform.localScale = Vector2.one;
    }
}
