using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EvaluableNoise : IEvaluable
{
    public float scale, offsetx, offsety;
    public ColorVec EvaluateColor(float vector = 0)
    {
        return EvaluateValue(vector);
    }

    public float EvaluateValue(float vector = 0)
    {
        return Mathf.PerlinNoise((vector + offsetx) * scale, offsety * scale);
    }

    public object GetCopy()
    {
        return new EvaluableNoise
        {
            scale = scale,
            offsetx = offsetx,
            offsety = offsety
        };
    }

    public int GetResolution()
    {
        return 256;
    }
}
