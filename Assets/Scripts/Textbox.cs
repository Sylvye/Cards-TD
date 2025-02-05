using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Textbox : MonoBehaviour
{
    [DoNotSerialize]
    public TMPLabel text;

    // Start is called before the first frame update
    void Start()
    {
        text = transform.GetChild(0).GetChild(0).GetComponent<TMPLabel>();
    }
}
