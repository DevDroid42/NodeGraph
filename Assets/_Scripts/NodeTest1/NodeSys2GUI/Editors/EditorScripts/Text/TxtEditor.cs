using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using nodeSys2;
using System;

public class TxtEditor : EditorBase
{
    public StringData stringData;
    public InputField inputField;
    private Property prop;
    // Start is called before the first frame update
    void Start()
    {

    }

    public void UpdateField(string data)
    {
        inputField.text = data;
    }

    public void UpdateGUI()
    {
        GUIGraph.updateGraphGUI.Invoke();
    }

    public void EditString(string str)
    {
        stringData.txt = str;
    }

    public void EditFloat(string num)
    {
        if (num.Length > 0)
        {
            if (float.TryParse(num, out float data))
            {
                prop.SetData(new EvaluableFloat(data));
            }            
        }
    }

    public override void Setup(Property prop)
    {
        base.Setup(prop);
        this.prop = prop;
        switch (prop.GetData())
        {
            case IEvaluable evaluable:
                {
                    UpdateField(evaluable.EvaluateValue(0).ToString());
                    break;
                }
            case StringData str:
                {
                    UpdateField(str.txt);
                    stringData = str;
                    break;
                }
            default:
                {
                    UpdateField(prop.GetData().ToString());
                    break;
                }
        }
    }
}
