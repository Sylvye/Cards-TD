using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Main : MonoBehaviour
{
    public static int lives = 100;
    public int[] packs = { 0, 0, 0 }; // artisan, fighter, hoarder

    public static Stats playerStats;
    public static Stats enemyStats;

    public int mapLength;
    public static int mapLength_;

    public static GameObject hitboxReticle_;
    public GameObject hitboxReticle;

    public static GameObject towerRangeReticle_;
    public GameObject towerRangeReticle;

    public static LayerMask placementLayerMask_;
    public LayerMask placementLayerMask;

    public static LayerMask enemyLayerMask_;
    public LayerMask enemyLayerMask;

    public static Main main;

    private void Awake()
    {
        main = this;
        placementLayerMask_ = placementLayerMask;
        hitboxReticle_ = hitboxReticle;
        towerRangeReticle_ = towerRangeReticle;
        enemyLayerMask_ = enemyLayerMask;
        mapLength_ = mapLength;
        playerStats = GetComponent<Stats>();
    }

    // Start is called before the first frame update
    private void Start()
    {
        enemyStats = Spawner.main.stats;
        StartCoroutine(SetupMap());
        Cards.AddAllChildren(); // adds cards to deck
    }

    public static bool Damage(int amount)
    {
        lives -= amount;
        if (lives <= 0)
        {
            lives = 0;
            return true;
        }
        return false;
    }

    public static void Earn(int amount)
    {
        playerStats.AddToStat("currency", amount);
        if (playerStats.GetStat("currency") < 0) // shouldn't happen ever, but just in case.
        {
            playerStats.SetStat("currency", 0);
        }
    }

    public static void UpdatePackLabels()
    {
        for (int i = 0; i < 3; i++)
        {
            StageController.inventoryLabels.transform.GetChild(i+1).GetComponent<TMPLabel>().SetText("" + main.packs[i]);
        }
    }

    private void Update() 
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
        if (Input.GetKeyUp(KeyCode.Alpha6))
            Spawner.main.Send(6);
        if (Input.GetKeyUp(KeyCode.M))
            Earn(100);
        if (Input.GetKeyUp(KeyCode.B))
            Debug.Log("You have: " + playerStats.GetStat("currency") + " coins");
        if (Input.GetKeyUp(KeyCode.S))
        {
            Debug.Log("Player Stats:\n" + playerStats);
            Debug.Log("Enemy Stats:\n" + enemyStats);
        }
    }

    private IEnumerator SetupMap() // waits a frame because im lazy and didnt want to reassign script execution order.
    {
        yield return null;
        MapController.GenerateMap(mapLength);
    }
}
