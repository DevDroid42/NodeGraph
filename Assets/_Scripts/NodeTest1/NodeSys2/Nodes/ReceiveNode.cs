using nodeSys2;
using UnityEngine;

public class ReceiveNode : Node
{
    public bool newData;
    public object data;
    public ReceiveNode()
    {
        nodeDisc = "Viewer node";
        inputs = new Port[1];
        outputs = new Port[0];
        InitPorts();
    }

    public override void Handle(int index, object data)
    {
        Debug.Log(data);
        newData = true;
        this.data = data;
    }
}
