using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using nodeSys2;
using System;

public class ColorTableNode : Node
{
    public Property colorTable, interpolationType, clippingType, resolution, output;
    public List<Property> colors;

    public ColorTableNode(bool x)
    {
        base.nodeDisc = "Color Table";
        colors = new List<Property>(0);
        colorTable = CreateInputProperty("Color Table", false, new EvaluableColorTable(3));
        interpolationType = CreateInputProperty("Interpolation Mode", false, new EvaluableColorTable.InterpolationType());
        interpolationType.interactable = true;
        clippingType = CreateInputProperty("Clipping Mode", false, new EvaluableColorTable.ClippingMode());
        clippingType.interactable = true;
        resolution = CreateInputProperty("Resolution", false, new EvaluableFloat(3));
        resolution.interactable = true;
        output = CreateOutputProperty("Output");
    }

    public override void Init()
    {
        base.Init();        
        int setRes = (int)((Evaluable)resolution.GetData()).EvaluateValue(0, 0, 0, 0);
        //if the set resoltion is different than the current one resize the list by either removing excess data
        //or adding new data
        if (colors.Count != setRes)
        {
            int diff = setRes - colors.Count;
            if (diff > 0)
            {
                for (int i = 0; i < diff; i++)
                {
                    colors.Add(CreateInputProperty("Color:" + (colors.Count), true, new EvaluableColorVec(1), typeof(Evaluable)));
                    colors[colors.Count - 1].interactable = true;
                }
            }
            else
            {
                int intialSize = colors.Count;
                for (int i = intialSize - 1; i > intialSize - 1 + diff; i--)
                {
                    if (RemoveProperty(colors[i]))
                    {
                        colors.RemoveAt(i);
                    }
                }
            }
        }
        SetColors();
        ProccessEnums();
        output.Invoke(((Evaluable)colorTable.GetData()).GetCopy());
    }

    public override void Handle()
    {
        SetColors();
        output.Invoke(((Evaluable)colorTable.GetData()).GetCopy());
    }

    private void SetColors()
    {
        EvaluableColorTable table = (EvaluableColorTable)(colorTable.GetData());
        
        if (colors.Count != table.GetkeyAmt())
        {
            Debug.Log("Reseting colorTable Resolution from: " + table.GetkeyAmt() + " to: " + colors.Count);
            colorTable.SetData(new EvaluableColorTable(colors.Count));
            table = (EvaluableColorTable)(colorTable.GetData());            
        }
        for (int i = 0; i < colors.Count; i++)
        {
            table.SetKey(i, ((Evaluable)colors[i].GetData()).EvaluateColor(0, 0, 0, 0));
        }        
    }

    private void ProccessEnums()
    {
        if (interpolationType.GetData().GetType() == typeof(string))
        {
            interpolationType.SetData(Enum.Parse(typeof(EvaluableColorTable.InterpolationType), (string)interpolationType.GetData()));
        }
        ((EvaluableColorTable)colorTable.GetData()).interType = (EvaluableColorTable.InterpolationType)interpolationType.GetData();
        if (clippingType.GetData().GetType() == typeof(string))
        {
            clippingType.SetData(Enum.Parse(typeof(EvaluableColorTable.ClippingMode), (string)clippingType.GetData()));
        }
        ((EvaluableColorTable)colorTable.GetData()).clipType = (EvaluableColorTable.ClippingMode)clippingType.GetData();
    }
}
