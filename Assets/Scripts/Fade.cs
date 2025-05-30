using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fade : MonoBehaviour
{
    public float delay = 0;
    public float fadeSpeed = 1;
    public bool destroyOnInvisible;
    private float time;
    private bool fading = true;
    private SpriteRenderer sr;

    // Start is called before the first frame update
    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        if (delay == 0)
        {
            fading = true;
        }
    }

    // Update is called once per frame
    private void Update()
    {
        time += Time.deltaTime;
        if (time >= delay)
        {
            fading = true;
        }

        if (fading)
        {
            Color c = sr.color;
            if (c.a - fadeSpeed * Time.deltaTime > 0)
            {
                sr.color = new Color(c.r, c.g, c.b, c.a - fadeSpeed * Time.deltaTime);
            }
            else if (destroyOnInvisible)
            {
                Destroy(gameObject);
            }
        }
    }
}
