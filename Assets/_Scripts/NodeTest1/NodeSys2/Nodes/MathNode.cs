using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using nodeSys2;

public class MathNode : Node
{
    [JsonProperty] private Property opTypeProp, elementCountProp, outputProp;
    [JsonProperty] private List<Property> elements;
    private EvaluableMath math;

    public MathNode(ColorVec pos) : base(pos)
    {
        base.nodeDisc = "Math";
        opTypeProp = CreateInputProperty("Mix Type", false, new EvaluableMath.OperationType());
        opTypeProp.interactable = true;
        elements = new List<Property>(0);
        elementCountProp = CreateInputProperty("Element Count", false, new EvaluableFloat(2));
        elementCountProp.interactable = true;
        outputProp = CreateOutputProperty("output");
    }

    public override void Init()
    {
        base.Init();
        EnumUtils.ConvertEnum<EvaluableMath.OperationType>(opTypeProp);
        EvaluableMath.OperationType opType = ((EvaluableMath.OperationType)opTypeProp.GetData());
        if (opType == EvaluableMath.OperationType.Sin || opType == EvaluableMath.OperationType.Cos ||
            opType == EvaluableMath.OperationType.Tan)
        {
            elementCountProp.SetData(new EvaluableFloat(1));
        }
        ProcessRes();
    }

    public override void Init2()
    {
        UpdateMixRGB();
        outputProp.Invoke(math);
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

    public override void Handle()
    {
        UpdateMixRGB();
        outputProp.Invoke(math);
    }

    private void UpdateMixRGB()
    {
        if (math == null || math.elements.Count != elements.Count)
        {
            math = new EvaluableMath();
            for (int i = 0; i < elements.Count; i++)
            {
                math.elements.Add((IEvaluable)elements[i].GetData());
            }
        }
        else
        {
            for (int i = 0; i < math.elements.Count; i++)
            {
                math.elements[i] = (IEvaluable)elements[i].GetData();
            }

        }
        math.opType = (EvaluableMath.OperationType)opTypeProp.GetData();
    }
}
