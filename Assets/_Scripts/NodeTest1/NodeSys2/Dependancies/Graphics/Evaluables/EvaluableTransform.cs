using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EvaluableTransform : IEvaluable
{
    //offset applied pre-scaling
    public float localOffset = 0;
    //offset applied post-scaling
    public float globalOffset = 0;
    public float scale = 1;
    public float pivot = 0;

    public IEvaluable child;

    //to be used for vector transformations in subclasses
    private float TransformVector(float input)
    {
        //avoid divide by zero error
        if (scale == 0)
        {
            input = 0.00001f;
        }
        //transform the vector by the global offset
        input = input - globalOffset;
        input = (input - pivot) * (1 / scale) + pivot;
        input = input - localOffset;
        return input;
    }

    public ColorVec EvaluateColor(float vector)
    {
        vector = TransformVector(vector);
        return child.EvaluateColor(vector);
    }

    public float EvaluateValue(float vector)
    {
        vector = TransformVector(vector);
        return child.EvaluateValue(vector);
    }

    public object GetCopy()
    {
        EvaluableTransform copy = new EvaluableTransform();
        copy.child = (IEvaluable)child.GetCopy();
        copy.localOffset = localOffset;
        copy.globalOffset = globalOffset;
        copy.scale = scale;
        copy.pivot = pivot;
        return copy;
    }

    public int GetResolution()
    {
        return child.GetResolution();
    }

   
}
