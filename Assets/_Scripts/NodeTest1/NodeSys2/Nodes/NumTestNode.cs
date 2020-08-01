using nodeSys2;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumTestNode : Node
{
    // Start is called before the first frame update
    public NumTestNode()
    {
        nodeDisc = "NumberTestNode";
        inputs = new Port[0];
        outputs = new Port[1];
        base.InitPorts();        
    }

    public void InvokeNum(int num)
    {
        outputs[0].Invoke(num);
    }

    
}
