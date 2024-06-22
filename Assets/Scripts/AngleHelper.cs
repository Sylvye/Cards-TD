using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngleHelper
{
    public static Vector2 DegreesToVector(float degrees)
    {
        return new Vector2(Mathf.Cos(degrees * Mathf.Deg2Rad), Mathf.Sin(degrees * Mathf.Deg2Rad));
    }

    public static float VectorToDegrees(Vector2 dir)
    {
        return Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
    }

    public static Vector2 RadiansToVector(float radians)
    {
        return new Vector2(Mathf.Cos(radians), Mathf.Sin(radians));
    }

    public static float VectorToRadians(Vector2 dir)
    {
        return Mathf.Atan2(dir.y, dir.x);
    }
}
