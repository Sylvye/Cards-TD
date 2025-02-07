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

    public static Stats playerStats = new();
    public static Stats enemyStats = new();

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

    // Start is called before the first frame update
    private void Start()
    {
        main = this;
        placementLayerMask_ = placementLayerMask;
        hitboxReticle_ = hitboxReticle;
        towerRangeReticle_ = towerRangeReticle;
        enemyLayerMask_ = enemyLayerMask;
        mapLength_ = mapLength;
        StartCoroutine(SetupMap());
        Cards.AddAllChildren(); // adds cards to deck

        InitEnemyStats();
        InitPlayerStats();
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
    }

    private IEnumerator SetupMap() // waits a frame because im lazy and didnt want to reassign script execution order.
    {
        yield return null;
        MapController.GenerateMap(mapLength_);
    }

    public void InitPlayerStats()
    {
        playerStats.ClearStats();
        playerStats.AddStat("currency", 0);
        playerStats.AddStat("base_damage", 0);
        playerStats.AddStat("flat_damage", 0);
        playerStats.AddStat("damage_mult", 1);
        playerStats.AddStat("energy_base_damage", 0);
        playerStats.AddStat("kinetic_base_damage", 0);
        playerStats.AddStat("energy_damage_mult", 1);
        playerStats.AddStat("kinetic_damage_mult", 1);
        playerStats.AddStat("pierce", 0);
        playerStats.AddStat("kinetic_pierce", 0);
        playerStats.AddStat("energy_pierce", 0);
        playerStats.AddStat("base_discount", 0);
    }

    public void InitEnemyStats()
    {
        enemyStats.AddStat("small_enemies", 5);
        enemyStats.AddStat("medium_enemies", 0);
        enemyStats.AddStat("large_enemies", 0);
        enemyStats.AddStat("small_enemy_speed", 2);
        enemyStats.AddStat("medium_enemy_speed", 1.5f);
        enemyStats.AddStat("large_enemy_speed", 1);
        enemyStats.AddStat("small_enemy_hp_mult", 1);
        enemyStats.AddStat("medium_enemy_hp_mult", 1);
        enemyStats.AddStat("large_enemy_hp_mult", 1);
        enemyStats.AddStat("wave_density", 1);
        enemyStats.AddStat("shields", 0);
        enemyStats.AddStat("desperation", 0); // under 50% = move #% faster
        enemyStats.AddStat("resistance", 1);
        enemyStats.AddStat("regeneration", 0);
    }
}
