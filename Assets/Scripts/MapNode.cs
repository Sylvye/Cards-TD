using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapNode : MonoBehaviour
{
    public int index;
    public int column;
    public MapNode[] exits;

    public Sprite spriteLight;
    public Sprite spriteDark;

    public string displayName;
    public StageController.Stage stage;

    private void OnMouseDown()
    {
        if (spriteDark != spriteLight && StageController.currentStage == StageController.Stage.Map)
        {
            transform.localScale = new Vector3(0.4f, 0.4f, 1);
            SetSprite(spriteLight);
        }
    }

    private void OnMouseUp()
    {
        transform.localScale = new Vector3(0.6f, 0.6f, 1);
    }

    private void OnMouseUpAsButton()
    {
        if (StageController.currentStage == StageController.Stage.Map)
        {
            if (spriteDark != spriteLight && column == MapController.currentNode.column + 1 && ((MapController.currentNode.exits[0] != null && MapController.currentNode.exits[0].Equals(this)) || MapController.currentNode.exits[1] != null && MapController.currentNode.exits[1].Equals(this)))
            {
                MapController.EliminateColumn(this);
                MapController.currentNode.SetSprite(MapController.nodeCompleted);
                MapController.currentNode.spriteLight = MapController.nodeCompleted;
                MapController.currentNode.spriteDark = MapController.nodeCompleted;
                MapController.currentNode = this;
                SetSprite(MapController.nodeCurrentDark);
                spriteDark = MapController.nodeCurrentDark;
                spriteLight = MapController.nodeCurrentLight;
                displayName = "Your Location";
            }
            if (Equals(MapController.currentNode))
            {
                StageController.SwitchStage(stage);
            }
        }
    }

    private void OnMouseOver()
    {
        if (spriteDark != spriteLight)
        {
            SetSpriteLight(true);
            if (exits.Length >= 1 && exits[0] != null)
            {
                exits[0].SetSpriteLight(true);
            }
            if (exits.Length >= 2 && exits[1] != null)
            {
                exits[1].SetSpriteLight(true);
            }
        }
    }

    private void OnMouseEnter()
    {
        if (MouseTooltip.GetText() != displayName)
            MouseTooltip.SetText(displayName);
        MouseTooltip.SetVisible(true);
    }


    private void OnMouseExit()
    {
        MouseTooltip.SetVisible(false);
        SetSpriteLight(false);
        if (exits.Length >= 1 && exits[0] != null)
        {
            exits[0].SetSpriteLight(false);
        }
        if (exits.Length >= 2 && exits[1] != null)
        {
            exits[1].SetSpriteLight(false);
        }
    }

    public void SetSpriteLight(bool b)
    {
        if (b)
            SetSprite(spriteLight);
        else
            SetSprite(spriteDark);
    }

    public void SetSprite(Sprite s)
    {
        GetComponent<SpriteRenderer>().sprite = s;
    }
}
