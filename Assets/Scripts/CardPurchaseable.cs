using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardPurchaseable : Button, Purchaseable
{
    public override void Action()
    {
        Claim();
        throw new System.NotImplementedException(); // SEE SCOPE CREEP EXTRAVAGANZA DOCUMENT FOR INSIGHTS
    }
    public void Claim()
    {

    }
}
