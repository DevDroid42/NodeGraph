using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using nodeSys2;
using Newtonsoft.Json;

public class FloatConstant : Node
{
    [JsonProperty] private Property constant, output;
    public FloatConstant(ColorVec pos) : base(pos)
    {
        nodeDisc = "Float Constant";
        constant = CreateInputProperty("const", false, new EvaluableFloat(0));
        constant.interactable = true;
        output = CreateOutputProperty("numOut");
    }

    public override void Init2()
    {
        base.Init2();
        output.Invoke((IEvaluable)constant.GetData());
    }
}
