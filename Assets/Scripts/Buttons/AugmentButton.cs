using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AugmentButton : Button
{
    // Update is called once per frame
    public override void OnUpdate()
    {
        SetClickable(AugmentTable.main.transform.GetChild(0).transform.childCount == 1 && AugmentTable.main.transform.GetChild(1).transform.childCount == 1);
    }

    public override void Action()
    {
        AugmentTable.Merge();
    }
}
