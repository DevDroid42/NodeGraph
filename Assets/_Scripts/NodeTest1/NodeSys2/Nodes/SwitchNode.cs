using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using nodeSys2;
using Newtonsoft.Json;

public class SwitchNode : Node
{
    [JsonProperty] private Property inputCountProp, currentElement, outputProp;
    [JsonProperty] private List<Property> elements;
    private EvaluableSwitch evaluableSwitch;

    public SwitchNode(ColorVec pos) : base(pos)
    {
        base.nodeDisc = "Switch";
        inputCountProp = CreateInputProperty("Input Count", false, new EvaluableFloat(2));
        inputCountProp.interactable = true;
        currentElement = CreateInputProperty("Current Element", true, new EvaluableFloat(0));
        currentElement.interactable = true;
        elements = new List<Property>();
        outputProp = CreateOutputProperty("output");
    }

    public override void Init()
    {
        ProcessRes();
    }

    public override void Init2()
    {
        UpdateSwitch();
        outputProp.Invoke(evaluableSwitch);
    }

    public override void Handle()
    {
        UpdateSwitch();
        outputProp.Invoke(evaluableSwitch);
    }


    private void ProcessRes()
    {
        int setRes = (int)((IEvaluable)inputCountProp.GetData()).EvaluateValue();
        //if the set resoltion is different than the current one resize the list by either removing excess data
        //or adding new data
        if (elements.Count != setRes)
        {
            int diff = setRes - elements.Count;
            if (diff > 0)
            {
                for (int i = 0; i < diff; i++)
                {
                    elements.Add(CreateInputProperty("element: " + (elements.Count), true, new EvaluableFloat(1)));
                    elements[elements.Count - 1].internalRepresentation = EditorTypeManagement.Editor.table;
                    elements[elements.Count - 1].currentEditor = EditorTypeManagement.Editor.number;
                    elements[elements.Count - 1].interactable = true;
                }
            }
            else
            {
                int intialSize = elements.Count;
                for (int i = intialSize - 1; i > intialSize - 1 + diff; i--)
                {
                    if (RemoveProperty(elements[i]))
                    {
                        elements.RemoveAt(i);
                    }
                }
            }
        }
    }

    private void UpdateSwitch()
    {
        if (evaluableSwitch == null || evaluableSwitch.elements.Count != elements.Count)
        {
            evaluableSwitch = new EvaluableSwitch();
            for (int i = 0; i < elements.Count; i++)
            {
                evaluableSwitch.elements.Add(elements[i].GetEvaluable());
            }
        }
        else
        {
            for (int i = 0; i < evaluableSwitch.elements.Count; i++)
            {
                evaluableSwitch.elements[i] = elements[i].GetEvaluable();
            }
        }
        evaluableSwitch.currentElement = (int)currentElement.GetEvaluable().EvaluateValue();
    }
}
