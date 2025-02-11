using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;


[Serializable]
public class Stat
{
    public float value;
    public string min;
    public string max;

    public Stat(float value, string min, string max)
    {
        this.value = value;
        this.min = min;
        this.max = max;
    }

    public float GetValue()
    {
        Clamp();
        return value;
    }

    public void SetValue(float val)
    {
        value = val;
        Clamp();
    }

    public void Modify(float val, Stats.Operation op)
    {
        switch (op)
        {
            case Stats.Operation.Add:
                value += val;
                break;
            case Stats.Operation.Subtract:
                value -= val;
                break;
            case Stats.Operation.Multiply:
                value *= val;
                break;
            case Stats.Operation.Divide:
                value /= val;
                break;
        }
        Clamp();
    }

    private void Clamp()
    {
        bool hasMin = float.TryParse(min, out float minimum);
        bool hasMax = float.TryParse(max, out float maximum);

        if (hasMin && value < minimum)
        {
            value = minimum;
        }
        else if (hasMax && value > maximum)
        {
            value = maximum;
        }
    }
}
