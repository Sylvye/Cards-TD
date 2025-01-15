using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Purchaseable
{
    public void Claim();

    public Sprite GetSprite();
}
