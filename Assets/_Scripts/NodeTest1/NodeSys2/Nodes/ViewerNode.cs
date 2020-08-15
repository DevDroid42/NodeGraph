using nodeSys2;
using UnityEngine;

public class ViewerNode : Node
{
    public object data = new object();
    public ViewerNode()
    {
        nodeDisc = "Viewer node";
        viewableData = new object[1];
        viewableDisc = new string[1];
        viewableData[0] = data;
        viewableDisc[0] = "data:";
        inputs = new Port[1];
        outputs = new Port[0];
        InitPorts();
    }

    public override void Handle(int index, object data)
    {
        Debug.Log(data);
        viewableData[0] = data;
        this.data = data;
    }
}
