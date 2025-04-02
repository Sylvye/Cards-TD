using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialAnimator : MonoBehaviour
{
    public string timeVar = "_time";
    public float speed;
    public bool animate = false;
    public float minVal= 0;
    public float maxVal = 1;
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
            Set(Mathf.Clamp(time, 0, 1));
            if (speed > 0 && time >= maxVal || speed < 0 && time <= maxVal)
            {
                animate = false;
                Set(maxVal);
            }
        }
    }

    public void Set(float val)
    {
        r.material.SetFloat(timeVar, val);
    }

    public void Set(string v, float val)
    {
        r.material.SetFloat(v, val);
    }

    public float Get()
    {
        return r.material.GetFloat(timeVar);
    }

    public float Get(string v)
    {
        return r.material.GetFloat(v);
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
        time = minVal;
        animate = false;
    }
}
