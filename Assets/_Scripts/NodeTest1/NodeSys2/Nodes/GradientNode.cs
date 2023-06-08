using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using nodeSys2;
using System;
using Newtonsoft.Json;

public class GradientNode : Node
{
    [JsonProperty] private Property interpolationType, resolution, output;
    //first prop is the position second is the color
    [JsonProperty] private List<(Property, Property)> keyProps;
    EvaluableGradient colorTable;

    public GradientNode(ColorVec pos) : base(pos)
    {
        base.nodeDisc = "Gradient";
        keyProps = new List<(Property, Property)>(0);
        interpolationType = CreateInputProperty("Interpolation Mode", false, new EvaluableColorTable.InterpolationType());
        interpolationType.interactable = true;
        resolution = CreateInputProperty("Resolution", false, new EvaluableFloat(3));
        resolution.interactable = true;
        output = CreateOutputProperty("Output");
    }

    public override void Init()
    {
        base.Init();
        EnumUtils.ConvertEnum<EvaluableColorTable.InterpolationType>(interpolationType);
        ProcessRes();
        SetColors();
    }

    public override void Init2()
    {
        base.Init2();
        output.Invoke(colorTable);
    }

    private void ProcessRes()
    {
        int setRes = (int)((IEvaluable)resolution.GetData()).EvaluateValue(0);
        //if the set resoltion is different than the current one resize the list by either removing excess data
        //or adding new data
        if (keyProps.Count != setRes)
        {
            int diff = setRes - keyProps.Count;
            if (diff > 0)
            {
                for (int i = 0; i < diff; i++)
                {
                    keyProps.Add(
                       (
                       CreateInputProperty("Position:" + (keyProps.Count), true, new EvaluableFloat(0.5f)),
                       CreateInputProperty("Color:" + (keyProps.Count), true, new EvaluableColorVec(1))
                       )
                    );
                    keyProps[keyProps.Count - 1].Item1.interactable = true;
                    keyProps[keyProps.Count - 1].Item2.interactable = true;
                }
            }
            else
            {
                int intialSize = keyProps.Count;
                for (int i = intialSize - 1; i > intialSize - 1 + diff; i--)
                {
                    if (RemoveProperty(keyProps[i].Item1) && RemoveProperty(keyProps[i].Item2))
                    {
                        keyProps.RemoveAt(i);
                    }
                }
            }
        }
    }

    public override void Handle()
    {
        SetColors();
        output.Invoke(colorTable);
    }

    private void SetColors()
    {
        if (colorTable == null || keyProps.Count != colorTable.GetkeyAmt())
        {
            //Debug.Log("Reseting gradient Resolution from: " + colorTable.GetkeyAmt() + " to: " + keyProps.Count);
            colorTable = new EvaluableGradient(keyProps.Count);
        }
        colorTable.interType = (EvaluableColorTable.InterpolationType)interpolationType.GetData();
        for (int i = 0; i < keyProps.Count; i++)
        {
            float position = keyProps[i].Item1.GetEvaluable().EvaluateValue();
            colorTable.SetKeyPositon(i, position);
            ColorVec color = keyProps[i].Item2.GetEvaluable().EvaluateColor();
            colorTable.SetKeyColor(i, color);
        }
    }
}
