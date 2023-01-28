using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvaluableBool : Evaluable
{
    private bool val;

    public EvaluableBool(bool val)
    {
        this.val = val;
    }

    public void Setval(bool number)
    {
        val = number;
    }

    public override ColorVec EvaluateColor(ColorVec vector)
    {
        return EvaluateValue(0);
    }

    public override float EvaluateValue(ColorVec vector)
    {
        if (val)
        {
            return 1;
        }
        else
        {
            return 0;
        }
    }

    public override object GetCopy()
    {
        return new EvaluableBool(val);
    }

    public override string ToString()
    {
        return val.ToString();
    }
}
