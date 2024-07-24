using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartButton : Button
{
    public override void Action()
    {
        if (active)
            Spawner.main.active = true;
    }
}
