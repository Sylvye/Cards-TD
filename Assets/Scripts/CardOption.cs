using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CardOption : Button
{
    public Card card;
    public LayerMask cardMask;

    public override void Action()
    {
        Cards.Add(card);
        StageController.SwitchStage("Map");
    }
}
