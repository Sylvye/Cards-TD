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

    public string type;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnMouseDown()
    {
        if (spriteDark != spriteLight)
            transform.localScale = new Vector3(0.4f, 0.4f, 1);
    }

    private void OnMouseUp()
    {
        if (spriteDark != spriteLight)
        {
            transform.localScale = new Vector3(0.6f, 0.6f, 1);
            if (column == MapController.currentNode.column + 1 && ((MapController.currentNode.exits[0] != null && MapController.currentNode.exits[0].Equals(this)) || MapController.currentNode.exits[1] != null && MapController.currentNode.exits[1].Equals(this))) 
            {
                MapController.EliminateColumn(this);
                MapController.currentNode.SetSprite(MapController.nodeCompleted);
                MapController.currentNode.spriteLight = MapController.nodeCompleted;
                MapController.currentNode.spriteDark = MapController.nodeCompleted;
                MapController.currentNode = this;
                SetSprite(MapController.nodeCurrentDark);
                spriteDark = MapController.nodeCurrentDark;
                spriteLight = MapController.nodeCurrentLight;
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
