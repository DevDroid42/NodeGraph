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
        SetupConstantsViewables(1, 0);
        IntData constant = new IntData(0);
        SetConstant(0, constant, "intConstant");
        InitPorts(0,1);        
    }

    public override void Init()
    {
        outputs[0].portDisc = constants[0].ToString();
        outputs[0].Invoke(((IntData)constants[0]).num);
        //Debug.Log("invoking :" + constants[0]);
    }


}
