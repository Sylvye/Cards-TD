using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaserTurretCard : Card
{
    public GameObject spawnable;

    public override void OnPlay()
    {
        Instantiate(spawnable, transform.position, Quaternion.identity);
    }
}
