using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimateMaterial : MonoBehaviour
{
    public string timeVar = "_time";
    public float speed;
    public bool animate = false;
    private Renderer r;
    private float time = 0;

    private void Awake()
    {
        r = GetComponent<Renderer>();
    }

    private void Start()
    {
        if (animate)
        {
            Activate();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (animate)
        {
            time += Time.unscaledDeltaTime * speed;
            SetTime(Mathf.Clamp(time, 0, 1));
            if (time >= 1)
            {
                animate = false;
            }
        }
    }

    public void SetTime(float time)
    {
        r.material.SetFloat(timeVar, time);
    }

    public float GetTime()
    {
        return r.material.GetFloat(timeVar);
    }

    public void Activate()
    {
        if (!animate)
        {
            animate = true;
        }
    }

    public void PrimeAnimation()
    {
        time = 0;
        animate = false;
    }
}
