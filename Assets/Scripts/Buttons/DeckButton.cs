using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class DeckButton : Button
{
    private GameObject deckText;
    private GameObject deckUI;
    private bool visible = false;

    // Start is called before the first frame update
    public override void OnAwake()
    {
        deckText = GameObject.Find("Deck Visualizer Text");
        deckUI = GameObject.Find("Deck Visualizer");
    }

    public override void OnStart()
    {
        deckText.SetActive(false);
        deckUI.SetActive(false);
    }

    public override void Action()
    {
        visible = !visible;
        deckText.SetActive(visible);
        deckUI.SetActive(visible);

        deckUI.transform.GetChild(0);

    }
}
