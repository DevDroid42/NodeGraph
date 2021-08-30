using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using nodeSys2;
using System;

public class MathNode : Node
{
    public Property expression, status, vector, variableCount, valueOutput, EquationOutput;
    public List<Property> variables;
    private EvaluableCustomEquation equation;

    public MathNode(bool x)
    {
        base.nodeDisc = "Math";
        variables = new List<Property>();
        expression = CreateInputProperty("expression", false, new StringData("0"));
        expression.interactable = true;
        status = CreateInputProperty("status", false, new Message("Evaluated: 0"));

        //CHANGE TO VECTOR TYPE AFTER EDITOR IS IMPLEMENTED
        vector = CreateInputProperty("vector", true, new EvaluableColorVec(0), typeof(Evaluable));
        variableCount = CreateInputProperty("Variable Count", false, new EvaluableFloat(0));
        variableCount.interactable = true;

        valueOutput = CreateOutputProperty("Value Output");
        EquationOutput = CreateOutputProperty("Equation Output");
    }

    private int setRes;
    public override void Init()
    {
        base.Init();
        setRes = (int)((Evaluable)variableCount.GetData()).EvaluateValue(0);
        InsantiateEquation();
        ProcessRes();
        EquationOutput.Invoke(equation.GetCopy());
        ValueOutput();
    }

    public override void Handle()
    {
        InsantiateEquation();
        ValueOutput();
        ProcessRes();
        EquationOutput.Invoke(equation.GetCopy());
    }

    private void InsantiateEquation()
    {
        equation = new EvaluableCustomEquation(setRes, ((StringData)expression.GetData()).txt, false);
    }

    private void ValueOutput()
    {
        if (equation.ToString() != "")
        {
            ColorVec localVector = ((Evaluable)vector.GetData()).EvaluateColor(0);
            float value;
            try
            {
                value = equation.EvaluateValue(localVector);
                status.SetData(new Message("Evaluated: " + value));
            }
            catch (Exception e)
            {
                Debug.LogWarning(e.ToString());
                status.SetData(new Message("Syntax Error"));
                value = 0;
            }
            valueOutput.Invoke(new EvaluableFloat(value));
        }
    }

    private void ProcessRes()
    {
        //if the set resoltion is different than the current one resize the list by either removing excess data
        //or adding new data
        if (variables.Count != setRes)
        {
            int diff = setRes - variables.Count;
            if (diff > 0)
            {
                for (int i = 0; i < diff; i++)
                {
                    variables.Add(CreateInputProperty("V" + (variables.Count), true, new EvaluableFloat(1), typeof(Evaluable)));
                    variables[variables.Count - 1].interactable = true;
                }
            }
            else
            {
                int intialSize = variables.Count;
                for (int i = intialSize - 1; i > intialSize - 1 + diff; i--)
                {
                    if (RemoveProperty(variables[i]))
                    {
                        variables.RemoveAt(i);
                    }
                }
            }
        }

        for (int i = 0; i < equation.variables.Length; i++)
        {
            equation.variables[i] = (Evaluable)variables[i].GetData();
        }

    }

}
