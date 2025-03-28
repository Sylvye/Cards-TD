using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroySelf : MonoBehaviour
{
    public float lifetime = -1;

    public void DeleteMe()
    {
        if (lifetime > 0)
        {
            Destroy(gameObject, lifetime);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
