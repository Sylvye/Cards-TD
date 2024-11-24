using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour
{
    public int lives = 100;
    public int currency = 0; // rename this later
    public int[] packs = { 0, 0, 0 }; // artisan, fighter, hoarder

    public int mapLength;
    public static int mapLength_;

    public Deckbuilder DB;

    public static GameObject hitboxReticle_;
    public GameObject hitboxReticle;

    public static GameObject towerRangeReticle_;
    public GameObject towerRangeReticle;

    public static LayerMask placementLayerMask_;
    public LayerMask placementLayerMask;

    public static LayerMask enemyLayerMask_;
    public LayerMask enemyLayerMask;

    public static Main main;

    // Start is called before the first frame update
    void Start()
    {
        main = this;
        placementLayerMask_ = placementLayerMask;
        hitboxReticle_ = hitboxReticle;
        towerRangeReticle_ = towerRangeReticle;
        enemyLayerMask_ = enemyLayerMask;
        mapLength_ = mapLength;
        StartCoroutine(SetupMap());
        DB.InitializeDeck();
    }

    public static bool Damage(int amount)
    {
        main.lives -= amount;
        if (main.lives <= 0)
        {
            main.lives = 0;
            return true;
        }
        return false;
    }

    public static void Earn(int amount)
    {
        main.currency -= amount;
        if (main.currency < 0) // shouldn't happen ever, but just in case.
        {
            main.currency = 0;
        }
    }

    public static void UpdatePackLabel()
    {
        TMPLabel label = GameObject.Find("Pack Label").GetComponent<TMPLabel>();
        label.SetText("Artisan: " + main.packs[0] + " Fighter: " + main.packs[1] + " Hoarder: " + main.packs[2]);
    }

    void Update() 
    {
        // testing purposes
        if (Input.GetKeyUp(KeyCode.Alpha1))
            Spawner.main.Send(1);
        if (Input.GetKeyUp(KeyCode.Alpha2))
            Spawner.main.Send(2);
        if (Input.GetKeyUp(KeyCode.Alpha3))
            Spawner.main.Send(3);
        if (Input.GetKeyUp(KeyCode.Alpha4))
            Spawner.main.Send(4);
        if (Input.GetKeyUp(KeyCode.Alpha5))
            Spawner.main.Send(5);
    }

    IEnumerator SetupMap()
    {
        yield return null;
        MapController.GenerateMap(mapLength_);
    }
}
