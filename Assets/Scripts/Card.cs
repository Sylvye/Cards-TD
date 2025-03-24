using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public abstract class Card : MonoBehaviour, CardInterface
{
    public string type;
    private Vector3 handPos;
    public float cooldown;
    [NonSerialized]
    public Stats stats;
    public static Transform playingField;

    public virtual void Awake()
    {
        playingField = GameObject.Find("Field").transform;
        stats = GetComponent<Stats>();
        GetComponent<SpriteRenderer>().sprite = GetSprite();
    }

    public virtual void Start()
    {
        
    }

    public virtual GameObject OnPlay()
    {
        return null;
    }

    private void OnMouseDown()
    {
        SetHandPos();
        transform.parent = transform.parent.parent;

        MouseDownAction();

        ActionButton.active = false;
        StageController.ToggleDarken(false);
        StageController.ToggleTime(true);
        Hand.Display(false);
    }

    private void OnMouseUp()
    {
        // this check is because something to do with the fact that this isnt a unity method
        if (Hand.GetIndexOf(this) != -1) // if card is in hand
        {
            if (!Spawner.main.IsStageCleared() && StageController.currentStage == StageController.Stage.Battle)
            {
                MouseUpAction();
            }
            else
            {
                ReturnToHand();
            }

            ActionButton.main.SetActive(true);
        }
    }

    private void OnMouseDrag()
    {
        if (StageController.currentStage == StageController.Stage.Battle)
        {
            MouseDragAction(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        }
        else
        {
            ReturnToHand();
        }
    }

    public abstract void MouseDownAction();

    public abstract void MouseUpAction();

    public virtual void MouseDragAction(Vector3 target)
    {
        transform.position = new Vector3(target.x, target.y, -6);
    }

    public virtual void ReturnToHand()
    {
        transform.parent = Hand.main.transform;
        transform.position = handPos;
        gameObject.SetActive(false);
    }

    public void SetHandPos()
    {
        handPos = (Vector2)transform.parent.position + Hand.GetIndexOf(this) * 2f * Vector2.right;
        handPos.z = -5;
    }

    public virtual string GetName()
    {
        return type;
    }

    public CardInterface FindReference(int index)
    {
        return Cards.GetFromDeck(index);
    }

    public int GetReferenceListLength()
    {
        return Cards.DeckSize();
    }

    public GameObject GetGameObject()
    {
        return gameObject;
    }

    public virtual Sprite GetSprite()
    {
        return GetSprite(1);
    }

    public virtual Sprite GetSprite(int tier)
    {
        return GetComponent<SpriteRenderer>().sprite;
    }

    public static void ClearField()
    {
        foreach (Transform child in playingField)
        {
            Destroy(child.gameObject);
        }
    }
}
