using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvaluableBlank : IEvaluable
{
    //Note: should always return a new color, not one with a reference inside the evaluable object
    public virtual ColorVec EvaluateColor(float vector)
    {
        return new ColorVec(0);
    }

    public virtual float EvaluateValue(float vector)
    {
        return 0;
    }

    public object GetCopy()
    {
        return new EvaluableBlank();
    }

    public int GetResolution()
    {
        return 1;
    }
}
