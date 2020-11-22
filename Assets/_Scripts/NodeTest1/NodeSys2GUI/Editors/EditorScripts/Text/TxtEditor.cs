using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using nodeSys2;
using System;

public class TxtEditor : EditorBase
{
    public IntData intData;
    public StringData stringData;
    public EvaluableFloat floatData;
    public InputField inputField;
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

    public void EditInt(string num)
    {
        intData.num = int.Parse(num);
    }

    public void EditFloat(string num)
    {
        if (num.Length > 0)
        {
            floatData.SetNum(float.Parse(num));
        }
    }

    public override void Setup(Property prop)
    {
        base.Setup(prop);
        UpdateField(prop.GetData().ToString());
        switch (prop.GetData())
        {
            case EvaluableFloat num:
                {
                    floatData = num;
                    break;
                }
            case StringData str:
                {
                    stringData = str;
                    break;
                }
            default:
                {
                    break;
                }
        }
    }
}
