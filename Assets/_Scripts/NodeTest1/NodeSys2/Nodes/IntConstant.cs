using nodeSys2;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntConstant : Node
{    
    // Start is called before the first frame update
    public IntConstant()
    {
        nodeDisc = "IntConstant";
        constants = new object[1];
        constantsDisc = new string[1];
        IntData constant = new IntData(0);
        constantsDisc[0] = "IntConstant";
        constants[0] = constant;        
        InitPorts(0,1);        
    }

    public override void IntialInvoke()
    {
        outputs[0].Invoke(((IntData)constants[0]).num);
        Debug.Log("invoking :" + constants[0]);
    }


}
