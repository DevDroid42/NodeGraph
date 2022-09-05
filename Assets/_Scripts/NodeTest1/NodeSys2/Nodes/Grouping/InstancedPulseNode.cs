using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using nodeSys2;

public class InstancedPulseNode : Node
{
    public Property output;

    public InstancedPulseNode(ColorVec pos) : base(pos)
    {
        nodeDisc = "Instanced Pulse";
        output = CreateOutputProperty("output");
    }

    public override void Handle()
    {
        //Debug.Log("sending pulse from node");
        output.Invoke(new Pulse());        
    }
}

