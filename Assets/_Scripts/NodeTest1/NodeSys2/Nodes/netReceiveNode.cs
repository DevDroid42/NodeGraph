using nodeSys2;
using System;
using UnityEngine;
//using UnityEngine;

public class netReceiveNode : Node
{

    public Property ID, dataType, output;
    //the constructor needs to have a paramater so that the deserializer can use the default one
    public netReceiveNode(bool x)
    {
        base.nodeDisc = "Add Node";
        ID = base.CreateInputProperty("Num1", false, new EvaluableFloat(0));
        dataType = base.CreateInputProperty("Num2", false, new EvaluableFloat(0));
        output = base.CreateOutputProperty("output");
    }

    public override void Init()
    {
        base.Init();
        nodeNetDelagate -= ReceiveData;
        nodeNetDelagate += ReceiveData;
    }

    public override void Handle()
    {

    }

    public override void ReceiveData(NetworkMessage message)
    {
        Debug.Log(message);
    }

}
