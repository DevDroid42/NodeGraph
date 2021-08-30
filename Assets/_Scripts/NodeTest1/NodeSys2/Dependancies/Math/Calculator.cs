using StringExtensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Calculator
{
    //x,y,z, and w can all be used in a eqation like normal. variables can be used in an equation like (10 + v0) or (v12^2)
    public float x = 0, y = 0, z = 0, w = 0;
    public float[] variables;
    private ExpressionEvaluator evaluator;
    private string expression = "";
    private bool debug = false;

    public Calculator(int inputCount, string expression = "")
    {
        if (inputCount > 99)
        {
            Debug.LogWarning("Error, cannot use over 99 variables in an caculator object. Defaulting to 99");
            inputCount = 99;
        }
        variables = new float[inputCount];
        for (int i = 0; i < variables.Length; i++)
        {
            variables[i] = 0;
        }
        evaluator = new ExpressionEvaluator();
        this.expression = expression;
    }

    public void setExpression(string expresion)
    {
        this.expression = expresion;
    }

    public string GetExpression()
    {
        return expression;
    }

    public float Evaluate()
    {
        string subedExpression = SubVariables(expression);
        return float.Parse(evaluator.Evaluate(subedExpression));
    }

    public float Evaluate(ColorVec vector)
    {
        this.x = vector.rx;
        this.y = vector.gy;
        this.z = vector.bz;
        this.w = vector.aw;
        string subedExpression = SubVariables(expression);
        string evaluated = evaluator.Evaluate(subedExpression);
        return float.Parse(evaluated);
    }

    private string SubVariables(string expresion)
    {
        if (debug)
            Debug.Log("Substituting Variables: " + expresion);
        //=========== lazy null reference fix=======
        //expression is null sometimes. I suspect it's not throwing errors because of a try catch
        if (expresion != null)
        {
            if (expresion.Contains("x"))
            {
                int index = expresion.IndexOf("x");
                return SubVariables(evaluator.Sub(expresion, x.ToString(), index, index + 1));
            }
            else if (expresion.Contains("y"))
            {
                int index = expresion.IndexOf("y");
                return SubVariables(evaluator.Sub(expresion, y.ToString(), index, index + 1));
            }
            else if (expresion.Contains("z"))
            {
                int index = expresion.IndexOf("z");
                return SubVariables(evaluator.Sub(expresion, z.ToString(), index, index + 1));
            }
            else if (expresion.Contains("w"))
            {
                int index = expresion.IndexOf("w");
                return SubVariables(evaluator.Sub(expresion, w.ToString(), index, index + 1));
            }
            else if (expresion.Contains("v"))
            {
                int index = expresion.IndexOf("v");
                int varIndex = GetNumberAfterIndex(expresion, index);
                if (varIndex < 0)
                {
                    Debug.Log("Cannot have negative variable indexes. Defaulting to 0");
                    return 0.ToString();
                }
                if (variables.Length == 0)
                {
                    Debug.Log("No Variables are registered but an equation is trying to access variable: " + varIndex + " Defaulting to 0");
                    return 0.ToString();
                }
                if (varIndex > variables.Length)
                {
                    Debug.Log("Variable number " + varIndex + " Is not registered. Defaulting to 0");
                    return 0.ToString();
                }
                return SubVariables(evaluator.Sub(expresion, variables[varIndex].ToString(), index, index + varIndex.ToString().Length + 1));
            }
            else
            {
                return expresion;
            }
        }
        else
        {
            return "0";
        }

    }

    private int GetNumberAfterIndex(string expression, int index)
    {
        int lower = index, upper = 0;
        for (int i = index + 1; i < expression.Length; i++)
        {
            if (!evaluator.IsNumber(expression[i]))
            {
                upper = i - 1;
                break;
            }
            if (i == expression.Length - 1)
            {
                upper = i;
            }
        }        
        return int.Parse(expression.JavaSubstring(lower + 1, upper + 1));
    }
}
