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
    public string type;

    private void OnMouseDown()
    {
        if (spriteDark != spriteLight && StageController.stageIndex == 0)
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
        if (StageController.stageIndex == 0)
        {
            string output = type;
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
                type = "Your Location";
            }
            if (Equals(MapController.currentNode))
            {
                StageController.SwitchStage(output);
            }
        }
    }

    private void OnMouseOver()
    {
        if (spriteDark != spriteLight)
        {
            if (NodeLabel.main.GetText() != displayName)
                NodeLabel.main.SetText(displayName);
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
    private void OnMouseExit()
    {
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
