using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using nodeSys2;
using System.Linq;

public class EvaluableSwitch : IEvaluable
{
    public int currentElement;
    public List<IEvaluable> elements = new List<IEvaluable>();

    public ColorVec EvaluateColor(float vector = 0)
    {
        return elements[currentElement].EvaluateColor(vector);
    }

    public float EvaluateValue(float vector = 0)
    {
        return elements[Mathf.Clamp(currentElement, 0, elements.Count-1)].EvaluateValue(vector);
    }

    public object GetCopy()
    {
        EvaluableSwitch evaluableSwitch = new EvaluableSwitch();
        foreach (IEvaluable element in elements)
        {
            evaluableSwitch.elements.Add((IEvaluable)element.GetCopy());
        }
        return evaluableSwitch;
    }

    public int GetResolution()
    {
        return elements.Max(element => element.GetResolution());
    }
}
