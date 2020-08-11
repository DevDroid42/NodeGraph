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
        constants = new object[1];
        int intConst = 0;
        constants[0] = intConst;
        inputs = new Port[0];
        outputs = new Port[1];
        base.InitPorts();        
    }

    public override void IntialInvoke()
    {
        outputs[0].Invoke(constants[0]);
    }


}
