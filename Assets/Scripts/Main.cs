using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Main : MonoBehaviour
{
    public int lives = 100;
    public Deckbuilder DB;
    public Hand hand;
    public Spawner spawner;
    public static LayerMask placementLayerMask_;
    public LayerMask placementLayerMask;
    public LayerMask enemyLayerMask;
    public static LayerMask enemyLayerMask_;
    public GameObject hitboxReticle;
    public static GameObject hitboxReticle_;

    private static Main main;

    // Start is called before the first frame update
    void Start()
    {
        main = this;
        placementLayerMask_ = placementLayerMask;
        hitboxReticle_ = hitboxReticle;
        enemyLayerMask_ = enemyLayerMask;
        DB.InitializeDeck();
        hand.Deal();
        hand.DisplayCards();
        spawner.active = true;
    }

    public static void Damage(int amount)
    {
        main.lives -= amount;
        if (main.lives <= 0)
        {
            main.lives = 0;
            Debug.Log("died");
            Debug.Break();
        }
    }
}
