using nodeSys2;
using System;
using UnityEngine;

public class AddNode : Node
{

    Property num1, num2;
    //the constructor needs to have a paramater so that the deserializer can use the default one
    public AddNode(bool x)
    {
        base.nodeDisc = "Add Node";
        num1 = base.CreateInputProperty("Num1", true, new FloatData(0));
        num2 = base.CreateInputProperty("Num2", true, new FloatData(0));
    }

    public override void Handle()
    {
        Debug.Log("Received data" + num1.GetData().ToString());
    }

}
