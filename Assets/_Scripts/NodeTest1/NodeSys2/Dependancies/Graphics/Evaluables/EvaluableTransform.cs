using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EvaluableTransform : IEvaluable
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum OOBBehavior
    {
        continuous, tile, mirror
    }
    public OOBBehavior oobBehavior;

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

    private float OOBTransform(float x)
    {
        switch (oobBehavior)
        {
            case OOBBehavior.continuous:
                return x;
            case OOBBehavior.tile:
                x = x - (int)x;
                if (x < 0)
                {
                    x = x + 1;
                }
                return x;
            case OOBBehavior.mirror:
                int remain = ((int)x) % 2;
                x = x - (int)x;
                if (x < 0)
                {
                    x = x + 1;
                }
                if (remain != 0)
                {
                    x = -x + 1;
                }
                return x;
            default:
                return x;
        }
    }

    public ColorVec EvaluateColor(float vector)
    {
        vector = OOBTransform(TransformVector(vector));
        return child.EvaluateColor(vector);
    }

    public float EvaluateValue(float vector)
    {
        vector = OOBTransform(TransformVector(vector));
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
