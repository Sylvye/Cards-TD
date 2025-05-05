using AYellowpaper.SerializedCollections;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Card Data", menuName = "Card", order = 0)]
public class CardData : ScriptableObject
{
    public Sprite sprite;
    [SerializedDictionary("Name", "Stat")]
    public Stats stats;
}
