using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public struct Stat
{
    public float value;
    public float min;
    public float max;

    public Stat(float value, float min, float max)
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
        if (min != -1 && value < min)
        {
            value = min;
        }
        else if (max != -1 && value > max)
        {
            value = max;
        }
    }
}
