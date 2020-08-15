using nodeSys2;
using System;
using UnityEngine;

public class AddNode : Node
{
    private int? num0,num1;
    public AddNode()
    {
        nodeDisc = "Add Node";
        inputs = new Port[2];
        outputs = new Port[1];

        InitPorts();
    }

    public override void Handle(int index, object data)
    {
        switch (data)
        {
            case int a:
                Calculate(index, a);
                break;
            default:                
                break;
        }
    }

    private void Calculate(int index, int data)
    {
        switch (index)
        {
            case 0:
                num0 = data;
                break;
            case 1:
                num1 = data;
                break;
            default:
                break;
        }
        if(num0 != null && num1 != null)
        {
            outputs[0].Invoke(num0 + num1);
            Debug.Log("invoking after add:" + (num0 + num1));
        }
    }
}
