using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BackgroundMaterial", menuName = "BackgroundMaterial", order = 1)]
public class BackgroundMat : ScriptableObject
{
    public Color overlapColor;
    public Color highColor;
    public Color lowColor;
    public float blobStep;
    public float blobPower;
    public float blobDensity;
    public float shearStrength;
    public float gradientNoiseStep;
}
