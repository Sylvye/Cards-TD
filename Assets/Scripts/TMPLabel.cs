using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TMPLabel : MonoBehaviour
{
    private TextMeshProUGUI tmp;

    // Start is called before the first frame update
    private void Start()
    {
        tmp = GetComponent<TextMeshProUGUI>();
    }

    public void SetText(string text)
    {
        if (tmp == null)
        {
            tmp = GetComponent<TextMeshProUGUI>();
        }
        tmp.text = text;
    }

    public string GetText()
    {
        if (tmp == null)
        {
            tmp = GetComponent<TextMeshProUGUI>();
        }
        return tmp.text;
    }
}
