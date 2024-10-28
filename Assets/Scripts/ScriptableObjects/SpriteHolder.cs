using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SpriteHolder", menuName = "SpriteHolder", order = 1)]
public class SpriteHolder : ScriptableObject
{
    public Sprite[] sprites;
}
