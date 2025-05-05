using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public interface Puppet
{
    public Puppetable GetReference();
    public void SetReference(Puppetable p);
}
