using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ShopCard : Button
{
    public Card card;
    public LayerMask cardMask;

    public override void Action()
    {
        Cards.AddToDeck(card);
        StageController.SwitchStage("Map");
    }
}
