using AYellowpaper.SerializedCollections;
using AYellowpaper.SerializedCollections.Editor.Search;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Augment Data", menuName = "Augment", order = 0)]
public class AugmentData : ScriptableObject
{
    public Sprite sprite;
    [SerializedDictionary("Name", "Stat")]
    public Stats stats = new();
}
