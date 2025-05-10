using AYellowpaper.SerializedCollections;
using AYellowpaper.SerializedCollections.Editor.Search;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Scriptable Stat Item", order = 0)]
public class ScriptableStatItem : ScriptableObject
{
    public Sprite sprite;
    public string id;
    public string info;
    public GameObject towerObj;
    [SerializedDictionary("Name", "Stat")]
    public Stats stats = new();
}
