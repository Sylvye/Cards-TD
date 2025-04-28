using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Puppet : SpriteUIE
{
    protected Puppetable reference;
    protected MaterialAnimator ma;

    public override void OnAwake() 
    {
        ma = GetComponent<MaterialAnimator>();
        sr = GetComponent<SpriteRenderer>();
    }

    public void SetReference(Puppetable r)
    {
        reference = r;
        sr.sprite = r.GetSprite();
    }

    public Puppetable GetReference() 
    { 
        return reference; 
    }

    public static Puppet MakePuppet(Card c)
    {
        Puppet p = new GameObject().GetComponent<Puppet>();
        p.SetReference(c);
        return p;
    }
}
