using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrageTurretCard : Card
{
    public GameObject spawnable;

    public override GameObject OnPlay()
    {
        return Instantiate(spawnable, transform.position, Quaternion.identity);
    }
}
