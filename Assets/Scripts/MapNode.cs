using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MapNode : Button
{
    public int index;
    public int column;
    public int spriteIndex;
    public Sprite spriteDeactivated;
    public MapNode[] exits;
    public bool deactivated = false;
    public StageController.Stage mode; // stage to send to when clicked

    public override void OnStart()
    {
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
        if (column == MapController.col && ((column == 0) ||
                    (MapController.currentNode.exits[0] != null
                    && MapController.currentNode.exits[0].Equals(this))
                    || MapController.currentNode.exits[1] != null
                    && MapController.currentNode.exits[1].Equals(this)))
        {
            StageController.SwitchStage(mode);

            MapController.EliminateColumn(this);
            MapController.currentNode.info = "trail";
            MapController.currentNode = this;
            MapController.col++;

            deactivated = true;
            ToggleOutline(false);
            OnMouseExit();
            SetSprite(spriteIndex + 2);
            info = "your Location";
        }
    }

    public override void OnMouseEnter()
    {
        if (!deactivated)
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
            if (MouseTooltip.GetText() != info)
                MouseTooltip.SetText(info);
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
        if (!deactivated)
        {
            base.OnMouseDown();
        }
    }

    public override void OnMouseUpAsButton()
    {
        if (!deactivated)
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
            SetSprite(MapController.sprites[spriteIndex]);
        }
        spriteUp = MapController.sprites[spriteIndex];
        spriteDown = MapController.sprites[spriteIndex + 1];
    }

    public override bool IsClickable()
    {
        return base.IsClickable() && !deactivated;
    }
}
