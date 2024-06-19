using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RareCard : Card
{
    public GameObject spawnable;

    private void Start()
    {
        radius = 0.75f;
    }

    public override void OnPlay()
    {
        Instantiate(spawnable, transform.position, Quaternion.identity);
    }
}
