using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Puppetable
{
    public string GetName();

    public string GetInfo();

    public Sprite GetSprite();

    public Puppet MakePuppet();
}
