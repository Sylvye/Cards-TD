using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public List<GameObject> wave;
    public float density = 1;
    public bool active = false;
    private float cooldown = 0;

    // Update is called once per frame
    void Update()
    {
        if (active)
        {
            if (wave.Count > 0)
            {
                cooldown -= Time.deltaTime;

                if (cooldown <= 0)
                {
                    if (wave[0] != null)
                    {
                        cooldown = 1 / density;
                        Instantiate(wave[0], transform.position, wave[0].transform.rotation);
                        wave.RemoveAt(0);
                    }
                    else
                    {
                        cooldown = 1 / density; // later, will make a "wait" class that stores a value that is added to the cooldown.
                    }
                }
            }
            else
            {
                active = false;
                cooldown = 0;
            }
        }
    }
}
