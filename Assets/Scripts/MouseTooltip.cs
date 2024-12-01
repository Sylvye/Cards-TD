using System.Collections;
using System.Collections.Generic;
using System.Reflection.Emit;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MouseTooltip : MonoBehaviour
{
    public static MouseTooltip main;
    private Canvas canvas;
    private TMPLabel label;
    private TextMeshProUGUI tmp;

    // Start is called before the first frame update
    void Start()
    {
        main = this;
        canvas = GetComponentInParent<Canvas>();
        label = GetComponent<TMPLabel>();
        tmp = GetComponent<TextMeshProUGUI>();
        SetVisible(false);
    }

    // Update is called once per frame
    void Update()
    {
        // moves the tooltip when it is visible
        if (tmp.enabled)
        {
            Vector2 pos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, Input.mousePosition, canvas.worldCamera, out pos);
            transform.position = canvas.transform.TransformPoint(pos);
        }
    }

    public static void SetText(string text)
    {
        main.label.SetText(text);
    }

    public static string GetText()
    {
        return main.label.GetText();
    }

    public static void SetVisible(bool visible)
    {
        main.tmp.enabled = visible;
    }
}
