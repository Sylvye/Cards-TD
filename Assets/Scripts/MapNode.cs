using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapNode : Button
{
    public int index;
    public int column;
    public int spriteIndex;
    public bool clickable = true;
    public Sprite spriteDeactivated;
    public MapNode[] exits;

    public string displayName;
    public StageController.Stage stage;

    public override void Action()
    {
        if (clickable && (column == MapController.currentNode.column + 1 && ((MapController.currentNode.exits[0] != null && MapController.currentNode.exits[0].Equals(this)) || MapController.currentNode.exits[1] != null && MapController.currentNode.exits[1].Equals(this)) || column == 0))
        {
            StageController.SwitchStage(stage);

            MapController.EliminateColumn(this);
            MapController.currentNode.SetSprite(MapController.nodeCompleted);
            MapController.currentNode.clickable = false;
            MapController.currentNode.displayName = "Trail";
            MapController.currentNode = this;
            SetSprite(MapController.currentNode.spriteIndex);
            displayName = "Your Location";
        }
    }

    private void OnMouseOver()
    {
        if (clickable)
        {
            if (exits.Length >= 1 && exits[0] != null)
            {
                exits[0].MakeSpriteDown();
            }
            if (exits.Length >= 2 && exits[1] != null)
            {
                exits[1].MakeSpriteDown();
            }
        }
    }

    public override void OnMouseEnter()
    {
        base.OnMouseEnter();
        if (MouseTooltip.GetText() != displayName)
            MouseTooltip.SetText(displayName);
        MouseTooltip.SetVisible(true);
    }


    public override void OnMouseExit()
    {
        base.OnMouseExit();
        MouseTooltip.SetVisible(false);
        if (exits.Length >= 1 && exits[0] != null)
        {
            exits[0].MakeSpriteUp();
        }
        if (exits.Length >= 2 && exits[1] != null)
        {
            exits[1].MakeSpriteUp();
        }
    }

    public void SetSprite(int spriteIndex)
    {
        SetSprite(spriteIndex, true);
    }

    public void SetSprite(int spriteIndex, bool set)
    {
        this.spriteIndex = spriteIndex;
        if (set)
        {
            GetComponent<SpriteRenderer>().sprite = MapController.sprites[spriteIndex];
        }
        spriteUp = MapController.sprites[spriteIndex];
        spriteDown = MapController.sprites[spriteIndex + 1];
    }
}
