using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using nodeSys2;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;

public class LogicNode : Node
{
    [JsonProperty] private Property data1, compareMode, data2, epsilon, pulseOutput, valueOutput;

    [JsonConverter(typeof(StringEnumConverter))]
    public enum ComparisonType
    {
        GreaterThan, LessThan, Within, Or, And
    }

    public LogicNode(ColorVec pos) : base(pos)
    {
        base.nodeDisc = "Logic";
        data1 = CreateInputProperty("data1", true, new EvaluableFloat(0));
        data1.interactable = true;
        epsilon = CreateInputProperty("epsilon", false, new EvaluableFloat(0.1f));
        compareMode = CreateInputProperty("Compare Mode", false, new ComparisonType());
        compareMode.interactable = true;
        data2 = CreateInputProperty("data2", true, new EvaluableFloat(0));
        data2.interactable = true;
        pulseOutput = CreateOutputProperty("Pulse Output");        
        valueOutput = CreateOutputProperty("Value Output");        
    }

    public override void Init()
    {
        base.Init();
        EnumUtils.ConvertEnum<ComparisonType>(compareMode);
        //only show epsilon for equality
        epsilon.visible = (ComparisonType)compareMode.GetData() == ComparisonType.Within;   
    }

    public override void Init2()
    {
        base.Init2();
        Handle();
    }


    public override void Handle()
    {
        switch ((ComparisonType)compareMode.GetData())
        {
            case ComparisonType.GreaterThan:
                Eval(data1.GetEvaluable().EvaluateValue(0) > ((IEvaluable)data2.GetData()).EvaluateValue(0));           
                break;
            case ComparisonType.LessThan:
                Eval(data1.GetEvaluable().EvaluateValue(0) < ((IEvaluable)data2.GetData()).EvaluateValue(0));
                break;
            case ComparisonType.Within:
                Eval(Math.Abs(data1.GetEvaluable().EvaluateValue(0) - data2.GetEvaluable().EvaluateValue(0)) < epsilon.GetEvaluable().EvaluateValue(0));
                break;
            case ComparisonType.Or:
                Eval(((bool)data1.GetEvaluable().EvaluateColor(0)) || (bool)(((IEvaluable)data2.GetData()).EvaluateColor(0)));
                break;
            case ComparisonType.And:
                Eval(((bool)data1.GetEvaluable().EvaluateColor(0)) && (bool)(((IEvaluable)data2.GetData()).EvaluateColor(0)));
                break;
            default:
                break;
        }
    }

    //used to tell if we are changing to rising
    private bool previousEvaluation = false;
    private void Eval(bool evaluation)
    {
        if (evaluation)
        {
            if (previousEvaluation == false)
            {
                pulseOutput.Invoke(new Pulse());
            }
            previousEvaluation = true;
            valueOutput.Invoke(new EvaluableColorVec(1));
        }
        else
        {
            valueOutput.Invoke(new EvaluableColorVec(0));
            previousEvaluation = false;
        }
    }
}
