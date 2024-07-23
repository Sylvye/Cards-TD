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

    // 0=battle, 1=deckbuild
    public static int mode = 0;

    public static GameObject hitboxReticle_;
    public GameObject hitboxReticle;

    public static LayerMask placementLayerMask_;
    public LayerMask placementLayerMask;

    public static LayerMask enemyLayerMask_;
    public LayerMask enemyLayerMask;

    private static Vector3 destination = new(0, 0, -10);

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
            SwitchStage(1);
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
    public static void SwitchStage(int stageIndex)
    {
        switch (stageIndex)
        {
            case 0:
                destination = new Vector3(0, 0, -10);
                mode = 0;
                Spawner.main.complete = false;
                break;
            case 1:
                destination = new Vector3(0, -8, -10);
                mode = 1;
                //DeckbuilderHelper.main.SetupOptions();
                MapController.GenerateMap(mapLength_);
                break;
        }
        
    }
}
