using nodeSys2;

public class ReceiveNode : Node
{
    public bool newData;
    public object data;
    public ReceiveNode()
    {
        inputs = new Port[1];
        outputs = new Port[0];
        InitPorts();
    }

    public override void Handle(int index, object data)
    {
        newData = true;
        this.data = data;
    }
}
