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


    void OnMouseUpAsButton()
    {
        Deck.Add(card);
        SceneManager.LoadScene(0);
        SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(0));
    }
}
