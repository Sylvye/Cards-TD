using AYellowpaper.SerializedCollections;
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
    public static Main main;

    public static int lives = 100;
    public int[] packs = { 0, 0 }; // augment, currency

    [SerializedDictionary("Name", "Stat")]
    public static Stats playerStats;
    [SerializedDictionary("Name", "Stat")]
    public static Stats enemyStats;

    public static Transform battlefield;

    public int mapLength;
    public static int mapLength_;

    public GameObject sparkles;
    public GameObject cardPuppet;

    public GameObject towerHitboxReticle;
    public GameObject towerRangeReticle;

    public LayerMask placementLayerMask;
    public LayerMask enemyLayerMask;

    private static TMPLabel coinLabel;


    private void Awake()
    {
        main = this;
        mapLength_ = mapLength;
        playerStats = new Stats();
        coinLabel = GameObject.Find("Coin label").GetComponent<TMPLabel>();
        battlefield = GameObject.Find("Field").transform;
    }

    // Start is called before the first frame update
    private void Start()
    {
        enemyStats = Spawner.main.stats;
        StartCoroutine(SetupMap());

        Cards.AddToDeck(new TowerCard("Assault Turret", Cards.main.cardLP.items[0].towerObj, 1, 0, Cards.main.cardLP.items[0].stats));
        Cards.AddToDeck(new TowerCard("Assault Turret", Cards.main.cardLP.items[0].towerObj, 1, 0, Cards.main.cardLP.items[0].stats));
        Cards.AddToDeck(new TowerCard("Assault Turret", Cards.main.cardLP.items[0].towerObj, 1, 0, Cards.main.cardLP.items[0].stats));
        Cards.AddToDeck(new TowerCard("Barrager", Cards.main.cardLP.items[1].towerObj, 1, 0, Cards.main.cardLP.items[1].stats));
        Cards.AddToDeck(new TowerCard("Barrager", Cards.main.cardLP.items[1].towerObj, 1, 0, Cards.main.cardLP.items[1].stats));
        Cards.AddToDeck(new TowerCard("Barrager", Cards.main.cardLP.items[1].towerObj, 1, 0, Cards.main.cardLP.items[1].stats));
        Cards.AddToDeck(new TowerCard("Marksman", Cards.main.cardLP.items[2].towerObj, 1, 0, Cards.main.cardLP.items[2].stats));
        Cards.AddToDeck(new TowerCard("Marksman", Cards.main.cardLP.items[2].towerObj, 1, 0, Cards.main.cardLP.items[2].stats));
        Cards.AddToDeck(new TowerCard("Marksman", Cards.main.cardLP.items[2].towerObj, 1, 0, Cards.main.cardLP.items[2].stats));
        Cards.AddToDeck(new TowerCard("Rocket Launcher", Cards.main.cardLP.items[3].towerObj, 1, 0, Cards.main.cardLP.items[3].stats));
        Cards.AddToDeck(new TowerCard("Tesla", Cards.main.cardLP.items[4].towerObj, 1, 0, Cards.main.cardLP.items[4].stats));
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

    public static void ClearField()
    {
        foreach (Transform child in battlefield)
        {
            Destroy(child.gameObject);
        }
    }
}
