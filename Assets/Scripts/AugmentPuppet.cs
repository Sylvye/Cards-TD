using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AugmentPuppet : SpriteUIE, Puppet
{
    private Puppetable reference;

    public Puppetable GetReference()
    {
        return reference;
    }

    public void SetReference(Puppetable r)
    {
        reference = r;
        SetSprite(r.GetSprite());
    }
}
