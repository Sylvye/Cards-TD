using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TMPLabel : MonoBehaviour
{
    private TextMeshProUGUI tmp;

    // Start is called before the first frame update
    void Start()
    {
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
