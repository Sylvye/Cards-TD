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
    public static bool paused = false;
    public static int lives = 100;
    public int[] packs = { 0, 0 }; // augment, gold

    public static Stats playerStats;
    public static Stats enemyStats;

    public int mapLength;
    public static int mapLength_;

    public GameObject sparkles;

    public static GameObject towerHitboxReticle_;
    public GameObject hitboxReticle;

    public static GameObject towerRangeReticle_;
    public GameObject towerRangeReticle;

    public static LayerMask placementLayerMask_;
    public LayerMask placementLayerMask;

    public static LayerMask enemyLayerMask_;
    public LayerMask enemyLayerMask;

    private static TMPLabel coinLabel;

    public static Main main;

    private void Awake()
    {
        main = this;
        placementLayerMask_ = placementLayerMask;
        towerHitboxReticle_ = hitboxReticle;
        towerRangeReticle_ = towerRangeReticle;
        enemyLayerMask_ = enemyLayerMask;
        mapLength_ = mapLength;
        coinLabel = GameObject.Find("Coin label").GetComponent<TMPLabel>();
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
        playerStats.ModifyStat("currency", amount);
        if (playerStats.GetStat("currency") < 0) // shouldn't happen ever, but just in case.
        {
            playerStats.SetStat("currency", 0);
        }

        coinLabel.SetText(""+playerStats.GetStat("currency"));
        Instantiate(main.sparkles, Camera.main.transform.Find("Coin animation"));
    }

    private void Update() 
    {
        // testing purposes
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            paused = !paused;
            StageController.ToggleDarken(paused);
            StageController.ToggleTime(!paused, 0);
        }
        if (Input.GetKeyUp(KeyCode.Alpha1))
            Spawner.main.Send(1);
        if (Input.GetKeyUp(KeyCode.Alpha2))
            Spawner.main.Send(2);
        if (Input.GetKeyUp(KeyCode.Alpha3))
            Spawner.main.Send(3);
        if (Input.GetKeyUp(KeyCode.Alpha6))
            StageController.SwitchStage((StageController.Stage)(6 - 5));
        if (Input.GetKeyUp(KeyCode.Alpha7))
            StageController.SwitchStage((StageController.Stage)(7 - 5));
        if (Input.GetKeyUp(KeyCode.Alpha8))
            StageController.SwitchStage((StageController.Stage)(8 - 5));
        if (Input.GetKeyUp(KeyCode.Alpha9))
            StageController.SwitchStage((StageController.Stage)(9 - 5));
        if (Input.GetKeyUp(KeyCode.Alpha0))
            StageController.SwitchStage((StageController.Stage)(10 - 5));
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
