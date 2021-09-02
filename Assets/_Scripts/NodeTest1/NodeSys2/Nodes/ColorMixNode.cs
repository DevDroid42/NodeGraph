using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using nodeSys2;
using System;

public class ColorMixNode : Node
{
    public Property mixType, factor, elementCount, output;
    public List<Property> elements;

    public ColorMixNode(bool x)
    {
        base.nodeDisc = "Mix Node";
        elements = new List<Property>(0);
        mixType = CreateInputProperty("Mix Type", false, new EvaluableMixRGB.MixType());
        mixType.interactable = true;
        factor = CreateInputProperty("Factor", true, new EvaluableFloat(1), typeof(Evaluable));
        elementCount = CreateInputProperty("Element Count", false, new EvaluableFloat(2));
        elementCount.interactable = true;
        output = CreateOutputProperty("output");
    }

    public override void Init()
    {        
        base.Init();
        ProcessRes();
        ProcessEnums();
    }

    public override void Init2()
    {
        output.Invoke(CreateMixRGB());
    }

    private void ProcessRes()
    {
        int setRes = (int)((Evaluable)elementCount.GetData()).EvaluateValue(0);
        //if the set resoltion is different than the current one resize the list by either removing excess data
        //or adding new data
        if (elements.Count != setRes)
        {
            int diff = setRes - elements.Count;
            if (diff > 0)
            {
                for (int i = 0; i < diff; i++)
                {
                    elements.Add(CreateInputProperty("element: " + (elements.Count), true, new EvaluableColorVec(1), typeof(Evaluable)));
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
        output.Invoke(CreateMixRGB());
    }

    private EvaluableMixRGB CreateMixRGB()
    {
        EvaluableMixRGB mixRGB = new EvaluableMixRGB((Evaluable)factor.GetData());
        for (int i = 0; i < elements.Count; i++)
        {
            mixRGB.AddElement((Evaluable)elements[i].GetData());
        }
        mixRGB.mixType = ((EvaluableMixRGB.MixType)mixType.GetData());
        return mixRGB;
    }

    private void ProcessEnums()
    {
        if (mixType.GetData().GetType() == typeof(string))
        {            
            mixType.SetData(Enum.Parse(typeof(EvaluableMixRGB.MixType), (string)mixType.GetData()));
        }
    }


}
