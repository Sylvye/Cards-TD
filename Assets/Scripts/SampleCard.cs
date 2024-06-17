using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleCard : Card
{
    public override void OnPlay()
    {
        Debug.Log("Played sample card");
    }
}
