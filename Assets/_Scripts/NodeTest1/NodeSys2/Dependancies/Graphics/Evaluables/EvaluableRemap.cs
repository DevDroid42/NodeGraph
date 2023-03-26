using System;

public class EvaluableRemap : IEvaluable
{
    public IEvaluable input, map;

    public ColorVec EvaluateColor(float vector)
    {
        return map.EvaluateColor(input.EvaluateValue(vector));
    }

    public float EvaluateValue(float vector = 0)
    {
        return map.EvaluateValue(input.EvaluateValue(vector));
    }

    public object GetCopy()
    {
        EvaluableRemap evaluableRemap = new EvaluableRemap();
        evaluableRemap.input = (IEvaluable)input.GetCopy();
        evaluableRemap.map = (IEvaluable)map.GetCopy();
        return evaluableRemap;
    }

    public int GetResolution()
    {
        return Math.Max(input.GetResolution(), map.GetResolution());
    }
}
