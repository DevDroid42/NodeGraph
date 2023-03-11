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
    private bool lockGraphUpdate = true;

    public override void Setup(Property prop)
    {
        //prevent stack overflow. UpdateBool changes toggle state,
        //Unity calls updateGUI, which then in turn calls setup
        //lock graph update will prevent unity from calling
        //GUI update on setup
        lockGraphUpdate = true;
        base.Setup(prop);
        this.prop = prop;
        UpdateBool();
        lockGraphUpdate = false;
    }

    public void UpdateBool()
    {
        switch (prop.GetData())
        {
            case IEvaluable evaluable:
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
        if (lockGraphUpdate) return;
        GUIGraph.updateGraphGUI.Invoke();
    }

    public void SetBool(bool value)
    {
        if (prop == null) return;
        prop.SetData(new EvaluableBool(value));
    }
}
