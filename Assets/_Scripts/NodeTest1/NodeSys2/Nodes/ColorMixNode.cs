using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using nodeSys2;
using System;
using Newtonsoft.Json;

public class ColorMixNode : Node
{
    [JsonProperty] private Property mixTypeProp, factorProp, elementCountProp, outputProp;
    [JsonProperty] private List<Property> elements;

    public ColorMixNode(ColorVec pos) : base(pos)
    {
        base.nodeDisc = "Mix";
        elements = new List<Property>(0);
        mixTypeProp = CreateInputProperty("Mix Type", false, new EvaluableMixRGB.MixType());
        mixTypeProp.interactable = true;
        factorProp = CreateInputProperty("Factor", true, new EvaluableFloat(1));
        factorProp.internalRepresentation = EditorTypeManagement.Editor.table;
        elementCountProp = CreateInputProperty("Element Count", false, new EvaluableFloat(2));
        elementCountProp.interactable = true;
        outputProp = CreateOutputProperty("output");
    }

    public override void Init()
    {        
        base.Init();
        EnumUtils.ConvertEnum<EvaluableMixRGB.MixType>(mixTypeProp);
        ProcessRes();
    }

    public override void Init2()
    {
        outputProp.Invoke(CreateMixRGB());
    }

    private void ProcessRes()
    {
        int setRes = (int)((IEvaluable)elementCountProp.GetData()).EvaluateValue(0);
        //if the set resoltion is different than the current one resize the list by either removing excess data
        //or adding new data
        if (elements.Count != setRes)
        {
            int diff = setRes - elements.Count;
            if (diff > 0)
            {
                for (int i = 0; i < diff; i++)
                {
                    elements.Add(CreateInputProperty("element: " + (elements.Count), true, new EvaluableColorVec(1)));
                    elements[elements.Count - 1].internalRepresentation = EditorTypeManagement.Editor.table;
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

    public override void Handle()
    {
        outputProp.Invoke(CreateMixRGB());
    }

    private EvaluableMixRGB CreateMixRGB()
    {
        EvaluableMixRGB mixRGB = new EvaluableMixRGB((IEvaluable)factorProp.GetData());
        for (int i = 0; i < elements.Count; i++)
        {
            mixRGB.elements.Add((IEvaluable)elements[i].GetData());
        }
        mixRGB.mixType = (EvaluableMixRGB.MixType)mixTypeProp.GetData();
        return mixRGB;
    }

}
