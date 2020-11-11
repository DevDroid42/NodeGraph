using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using nodeSys2;
using Newtonsoft.Json;

public class FloatConstant : Node
{
    public Property constant, output;
    public FloatConstant(bool x)
    {
        nodeDisc = "Float Constant";
        constant = CreateInputProperty("const", false, new EvaluableFloat(0));
        constant.interactable = true;
        output = CreateOutputProperty("numOut");
    }

    public override void Init()
    {
        output.Invoke(constant.GetData());
    }
}
