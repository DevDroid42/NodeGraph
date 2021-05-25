using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using nodeSys2;

public class GroupOutputNode : Node
{
    public Property input;
    public delegate void GroupOutDelegate(object data);
    //used to connect to a parent group or instancer node
    [JsonIgnore] public GroupOutDelegate outDel;

    public GroupOutputNode(bool x)
    {
        base.nodeDisc = "Group output";
        input = CreateInputProperty("input", true, new Evaluable());
    }

    public override void Handle()
    {
        outDel.Invoke(input.GetData());
    }
}
