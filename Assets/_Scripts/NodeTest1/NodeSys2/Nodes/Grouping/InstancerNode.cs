using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using nodeSys2;

//similar functionality to group node but handles instances of graphs. Each instance will have it's outputs of same name mixed via 
//color mixing
public class InstancerNode : GroupNode
{
    public InstancerNode(bool x) : base(x)
    {

    }
}
