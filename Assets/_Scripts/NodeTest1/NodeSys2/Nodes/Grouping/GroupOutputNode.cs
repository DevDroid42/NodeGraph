using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using nodeSys2;

public class GroupOutputNode : Node, INameable
{
    public Property input, name;
    public delegate void GroupOutDelegate(object data, string tag);
    //used to connect to a parent group or instancer node
    [JsonIgnore] public GroupOutDelegate outDel;

    public GroupOutputNode(bool x)
    {
        base.nodeDisc = "Group output";
        name = CreateInputProperty("Data tag", false, new StringData("output"), typeof(StringData));
        name.interactable = true;
        input = CreateInputProperty("input", true, new Evaluable());
    }

    public override void Handle()
    {
        outDel.Invoke(input.GetData(), getName());
    }

    public string getName()
    {
        return ((StringData)name.GetData()).txt;
    }
}
