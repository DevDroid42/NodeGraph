using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using nodeSys2;

public class BooleanEditorScript : EditorBase
{
    public Toggle toggle;
    private Property prop;

    public override void Setup(Property prop)
    {
        base.Setup(prop);
        this.prop = prop;
        UpdateBool();
    }

    public void UpdateBool()
    {
        switch (prop.GetData())
        {
            case Evaluable evaluable:
                {
                    toggle.isOn = evaluable.EvaluateValue(0) >= 0.5;
                    break;
                }
            default:
                {
                    Debug.LogWarning("Invalid data assigned to color editor, Can not display type of:" + prop.GetData().GetType().Name);
                    break;
                }
        }

    }

    public void Update()
    {
        if (!toggle.interactable)
        {
            UpdateBool();
        }
    }

    public void UpdateGUI()
    {
        //GUIGraph.updateGraphGUI.Invoke();
    }

    public void SetBool(bool value)
    {
        if (prop == null) return;
        prop.SetData(new EvaluableBool(value));
    }
}
