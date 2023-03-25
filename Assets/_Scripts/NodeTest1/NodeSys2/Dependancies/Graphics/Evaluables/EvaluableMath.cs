using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EvaluableMath : IEvaluable
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum OperationType
    {
        Add, Subtract, Multiply, Divide, Power, Log, Sin, Cos, Tan
    }
    public OperationType opType;

    public List<IEvaluable> elements;

    public EvaluableMath()
    {
        elements = new List<IEvaluable>();
    }

    public ColorVec EvaluateColor(float vector)
    {
        switch (opType)
        {
            case OperationType.Add:
                return OperateColor(vector, (x, y) => x + y);
            case OperationType.Subtract:
                return OperateColor(vector, (x, y) => x - y);
            case OperationType.Multiply:
                return OperateColor(vector, (x, y) => x * y);
            case OperationType.Divide:
                return OperateColor(vector, (x, y) => x * (1 / y));
            default:
                return EvaluateValue(0);
        }
    }

    public float EvaluateValue(float vector)
    {
        switch (opType)
        {
            case OperationType.Add:
                return OperateValue(vector, (float x, float y) => x + y);
            case OperationType.Subtract:
                return OperateValue(vector, (x, y) => x - y);
            case OperationType.Multiply:
                return OperateValue(vector, (x, y) => x * y);
            case OperationType.Divide:
                return OperateValue(vector, (x, y) => x * (1 / y));
            case OperationType.Power:
                return OperateValue(vector, (x, y) => (float)Math.Pow(x, y));
            case OperationType.Log:
                return OperateValue(vector, (x, y) => (float)Math.Log(x, y));
            case OperationType.Sin:
                return (float)Math.Sin(elements[0].EvaluateValue(0));
            case OperationType.Cos:
                return (float)Math.Cos(elements[0].EvaluateValue(0));
            case OperationType.Tan:
                return (float)Math.Tan(elements[0].EvaluateValue(0));
            default:
                return 0;
        }
    }

    private ColorVec OperateColor(float vector, Func<ColorVec, ColorVec, ColorVec> operation)
    {
        if (elements.Count == 0) return default;
        ColorVec output = elements[0].EvaluateColor(vector);
        for (int i = 1; i < elements.Count; i++)
        {
            output = operation(output, elements[i].EvaluateColor(vector));
        }
        return output;
    }

    private float OperateValue(float vector, Func<float, float, float> operation)
    {
        if (elements.Count == 0) return default;
        float output = elements[0].EvaluateValue(vector);
        for (int i = 1; i < elements.Count; i++)
        {
            output = operation(output, elements[i].EvaluateValue(vector));
        }
        return output;
    }

    public object GetCopy()
    {
        EvaluableMath math = new EvaluableMath();
        math.opType = opType;
        for (int i = 0; i < elements.Count; i++)
        {
            math.elements.Add((IEvaluable)elements[i].GetCopy());
        }
        return math;
    }

    public int GetResolution()
    {
        return elements.Max(element => element.GetResolution());
    }
}
