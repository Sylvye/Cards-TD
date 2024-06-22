using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class KineticTurret : Turret
{
    private float lastShot = -999;

    // Update is called once per frame
    void Update()
    {
        if (lastShot + 1/attackSpeed <= Time.time)
        {
            Debug.Log("Ready");
            if (Shoot())
            {
                
                lastShot = Time.time;
            }
            else
            {
                Debug.Log("No enemies");
            }
        }
    }
}
