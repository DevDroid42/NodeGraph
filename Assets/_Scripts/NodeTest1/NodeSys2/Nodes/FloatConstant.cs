using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using nodeSys2;
public class FloatConstant : Node
{
    Property constant, output;
    public FloatConstant(bool x)
    {
        nodeDisc = "Float Constant";
        constant = CreateInputProperty("const", false, new FloatData(0));
        constant.interactable = true;
        output = CreateOutputProperty("numOut");
    }

    public override void Init()
    {
        output.dataPort.Invoke(constant.GetData());
    }
}
