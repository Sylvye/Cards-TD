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

    public int mapLength;
    public static int mapLength_;

    public Deckbuilder DB;
    public Hand hand;

    // 0=map, 1=battle, 2=shop, 3=augment, 4=upgrade
    public static int mode = 0;

    public static GameObject hitboxReticle_;
    public GameObject hitboxReticle;

    public static LayerMask placementLayerMask_;
    public LayerMask placementLayerMask;

    public static LayerMask enemyLayerMask_;
    public LayerMask enemyLayerMask;

    public static GameObject laser_;
    public GameObject laser;

    private static Vector3 destination = new(0, 0, -10);

    private static Main main;

    // Start is called before the first frame update
    void Start()
    {
        main = this;
        placementLayerMask_ = placementLayerMask;
        hitboxReticle_ = hitboxReticle;
        enemyLayerMask_ = enemyLayerMask;
        laser_ = laser;
        DB.InitializeDeck();
        hand.Deal();
        hand.DisplayCards();
        mapLength_ = mapLength;
    }

    public static void Damage(int amount)
    {
        main.lives -= amount;
        if (main.lives <= 0)
        {
            main.lives = 0;
            Debug.Break();
        }
    }

    void Update() 
    {
        if (GameObject.FindWithTag("Enemy") == null && Spawner.main.complete && mode == 0)
        {
            SwitchStage("Map");
        }

        // testing purposes
        if (Input.GetKeyUp(KeyCode.Alpha1))
            Spawner.main.Send(1);
        if (Input.GetKeyUp(KeyCode.Alpha2))
            Spawner.main.Send(2);
        if (Input.GetKeyUp(KeyCode.Alpha3))
            Spawner.main.Send(3);
        if (Input.GetKeyUp(KeyCode.Alpha4))
            Spawner.main.Send(4);

        if (Vector3.Distance(Camera.main.transform.position, destination) > 0.02f)
            Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, destination, Time.deltaTime*5);
        else
            Camera.main.transform.position = destination;
    }

    // 0 is battle
    // 1 is deckbuilding
    public static void SwitchStage(string name)
    {
        switch (name)
        {
            case "Map":
                destination = new Vector3(0, -8, -10);
                mode = 0;
                if (MapController.currentNode == null)
                    MapController.GenerateMap(mapLength_);
                break;
            case "Defense":
                destination = new Vector3(0, 0, -10);
                mode = 1;
                Spawner.main.complete = false;
                break;
            case "Shop":
                destination = new Vector3(0, -8, -10);
                mode = 2;
                ShopController.main.SetupOptions();
                break;
            case "Augment":
                destination = new Vector3(0, -8, -10);
                mode = 3;
                break;
            case "Upgrade":
                destination = new Vector3(0, -8, -10);
                mode = 4;
                break;
            default:
                break;
        }
        
    }
}
