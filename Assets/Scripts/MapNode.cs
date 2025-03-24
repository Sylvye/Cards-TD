using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapNode : Button
{
    public int index;
    public int column;
    public int spriteIndex;
    public Sprite spriteDeactivated;
    public MapNode[] exits;

    public string displayName;
    public StageController.Stage stage;

    public override void Start()
    {
        base.Start();
        foreach (MapNode node in exits)
        {
            if (node != null)
            {
                GameObject link = Instantiate(MapController.main.mapNodeLink, Vector3.forward * 0.5f, Quaternion.identity, transform);
                LineRenderer lr = link.GetComponent<LineRenderer>();
                Vector3[] points = { transform.position, node.transform.position };
                lr.SetPositions(points);
            }
        }
    }

    public override void Action()
    {
        if (GetActive() && 
            column == MapController.col && 
                ((column == 0) || 
                (MapController.currentNode.exits[0] != null 
                    && MapController.currentNode.exits[0].Equals(this)) 
                    || MapController.currentNode.exits[1] != null 
                    && MapController.currentNode.exits[1].Equals(this)))
        {
            StageController.SwitchStage(stage);

            MapController.EliminateColumn(this);
            MapController.currentNode.displayName = "trail";
            MapController.currentNode = this;
            MapController.col++;

            SetActive(false);
            ToggleOutline(false);
            OnMouseExit();
            SetSprite(spriteIndex + 2);
            displayName = "your Location";
        }
    }

    public override void OnMouseEnter()
    {
        if (GetActive())
        {
            base.OnMouseEnter();
            if (exits.Length >= 1 && exits[0] != null)
            {
                exits[0].ToggleOutline(true);
            }
            if (exits.Length >= 2 && exits[1] != null)
            {
                exits[1].ToggleOutline(true);
            }
            if (MouseTooltip.GetText() != displayName)
                MouseTooltip.SetText(displayName);
            MouseTooltip.SetVisible(true);
        }
    }


    public override void OnMouseExit()
    {
        base.OnMouseExit();
        MouseTooltip.SetVisible(false);
        if (exits.Length >= 1 && exits[0] != null)
        {
            exits[0].ToggleOutline(false);
        }
        if (exits.Length >= 2 && exits[1] != null)
        {
            exits[1].ToggleOutline(false);
        }
    }

    public override void OnMouseDown()
    {
        if (GetActive())
        {
            base.OnMouseDown();
        }
    }

    public override void OnMouseUpAsButton()
    {
        if (GetActive())
        {
            base.OnMouseUpAsButton();
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

    public override void SetActive(bool a)
    {
        base.SetActive(a, false);
    }
}
