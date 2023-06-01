using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using nodeSys2;

public class EvaluablePoint : IEvaluable
{
    public float start, end;
    public IEvaluable pointColor, backgroundColor;

    public ColorVec EvaluateColor(float vector = 0)
    {
        if(vector > start && vector < end)
        {
            float pos = (vector - start) / (end - start);
            return pointColor.EvaluateColor(pos);
        }
        else
        {
            return backgroundColor.EvaluateColor(vector);
        }
    }

    public float EvaluateValue(float vector = 0)
    {
        return (float)EvaluateColor(vector);
    }

    public object GetCopy()
    {
        return new EvaluablePoint()
        {
            start = start,
            end = end,
            pointColor = (IEvaluable)pointColor.GetCopy(),
            backgroundColor = (IEvaluable)backgroundColor.GetCopy(),
        };
    }

    public int GetResolution()
    {
        throw new System.NotImplementedException();
    }
}
