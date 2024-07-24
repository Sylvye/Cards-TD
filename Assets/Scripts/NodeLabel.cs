using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NodeLabel : MonoBehaviour
{
    public static NodeLabel main;
    private TextMeshProUGUI tmp;

    // Start is called before the first frame update
    void Start()
    {
        main = this;
        tmp = GetComponent<TextMeshProUGUI>();
    }

    public void SetText(string text)
    {
        tmp.text = text;
    }
    
    public string GetText()
    {
        return tmp.text;
    }
}
