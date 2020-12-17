using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class EvaluableCustomEquation : EvaluableEquation
{
    [JsonProperty]
    private Calculator calc;        
    public Evaluable[] variables;
    private bool errorChecking;

    public EvaluableCustomEquation(int variableCount, string Expression, bool errorChecking = true)
    {
        variables = new Evaluable[variableCount];
        for (int i = 0; i < variables.Length; i++)
        {
            variables[i] = new Evaluable();
        }
        calc = new Calculator(variableCount, Expression);
        this.errorChecking = errorChecking;
    }

    public override ColorVec EvaluateColor(float x, float y, float z, float w)
    {
        return EvaluateValue(x, y, z, w);
    }

    public override float EvaluateValue(float x, float y, float z, float w)
    {
        for (int i = 0; i < variables.Length; i++)
        {
            calc.variables[i] = variables[i].EvaluateValue(x, y, z, w);
        }

        if (errorChecking)
        {
            try
            {
                return calc.Evaluate(x, y, z, w);
            }
            catch (System.Exception e)
            {
                Debug.Log(e);
                return 0;
            }
        }
        else
        {
            return calc.Evaluate(x, y, z, w);
        }
        
    }

    public override Evaluable GetCopy()
    {
        EvaluableCustomEquation copy = new EvaluableCustomEquation(variables.Length, calc.GetExpression());
        for (int i = 0; i < copy.variables.Length; i++)
        {
            copy.variables[i] = variables[i].GetCopy();
        }
        return copy;
    }

    public override string ToString()
    {
        return calc.GetExpression();
    }
}
