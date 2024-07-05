using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CardOption : MonoBehaviour
{
    public Card card;
    public LayerMask cardMask;
    public bool active = true;

    private void Update()
    {
        if (Main.mode == 0)
        {
            Destroy(gameObject);
        }
    }

    void OnMouseUpAsButton()
    {
        Deck.Add(card);
        Main.SwitchStage(0);
    }
}
