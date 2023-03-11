using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class EvaluableCustomEquation : IEvaluable
{
    [JsonProperty]
    private Calculator calc;        
    public IEvaluable[] variables;
    private bool errorChecking;

    public EvaluableCustomEquation(int variableCount, string Expression, bool errorChecking = true)
    {
        variables = new IEvaluable[variableCount];
        for (int i = 0; i < variables.Length; i++)
        {
            variables[i] = new EvaluableBlank();
        }
        calc = new Calculator(variableCount, Expression);
        this.errorChecking = errorChecking;
    }

    public ColorVec EvaluateColor(float vector)
    {
        return EvaluateValue(vector);
    }

    public float EvaluateValue(float vector)
    {
        for (int i = 0; i < variables.Length; i++)
        {
            calc.variables[i] = variables[i].EvaluateValue(vector);
        }

        if (errorChecking)
        {
            try
            {
                return calc.Evaluate(vector);
            }
            catch (System.Exception e)
            {
                Debug.Log(e);
                return 0;
            }
        }
        else
        {
            return calc.Evaluate(vector);
        }
        
    }

    public object GetCopy()
    {
        EvaluableCustomEquation copy = new EvaluableCustomEquation(variables.Length, calc.GetExpression());
        for (int i = 0; i < copy.variables.Length; i++)
        {
            copy.variables[i] = (IEvaluable)variables[i].GetCopy();
        }
        return copy;
    }

    public override string ToString()
    {
        return calc.GetExpression();
    }

    public int GetResolution()
    {
        throw new System.NotImplementedException();
    }
}
