using nodeSys2;
using System;
using UnityEngine;
//using UnityEngine;

public class AddNode : Node
{

    public Property num1, num2, output;
    //the constructor needs to have a paramater so that the deserializer can use the default one
    public AddNode(ColorVec pos) : base(pos)
    {
        base.nodeDisc = "Add Node";
        num1 = base.CreateInputProperty("Num1", true, new EvaluableFloat(0));
        num2 = base.CreateInputProperty("Num2", true, new EvaluableFloat(0));
        output = base.CreateOutputProperty("output");
    }

    public override void Init()
    {
        base.Init();
    }

    //these are numbers that live processing will be done on
    private float float1, float2;
    public override void Handle()
    {
        ProcessData();
        output.Invoke(new EvaluableFloat(float1 + float2));
    }

    private void ProcessData()
    {
        Evaluable f = null;
        if (num1.TryGetDataType(ref f))
        {
            float1 = f.EvaluateValue(0);
        }

        if (num2.TryGetDataType(ref f))
        {
            float2 = f.EvaluateValue(0);
        }
    }

}
