using nodeSys2;
using System;
using UnityEngine;

public class AddNode : Node
{
    private int? num0,num1;
    public AddNode()
    {
        nodeDisc = "Add Node";
        InitPorts(2, 1);
        base.inputs[0].portDisc = "element 1";
        base.inputs[1].portDisc = "element 2";
        base.outputs[0].portDisc = "output";
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
                inputs[0].portDisc = data.ToString();
                break;
            case 1:
                num1 = data;
                inputs[1].portDisc = data.ToString();
                break;
            default:
                break;
        }
        if(num0 != null && num1 != null)
        {
            outputs[0].Invoke(num0 + num1);
            outputs[0].portDisc = (num0 + num1).ToString();
            Debug.Log("invoking after add:" + (num0 + num1));
        }
    }
}
