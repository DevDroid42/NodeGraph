using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using nodeSys2;

public class GroupInputNode : Node
{
    public Property input, name, output;

    public GroupInputNode(bool x)
    {
        base.nodeDisc = "Group Input";
        input = CreateInputProperty("DataIn", false, new Evaluable());
        input.visible = false;
        name = CreateInputProperty("Data tag", false, new StringData("input"));
        output = CreateOutputProperty("output");
    }

    public override void Handle()
    {
        output.Invoke(input.GetData());
    }
}
