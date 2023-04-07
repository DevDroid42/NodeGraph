using Newtonsoft.Json;
using nodeSys2;
using System;
using System.Text;
using UnityEngine;
//using UnityEngine;

public class NetReceiveNode : Node, INetReceivable
{

    //[JsonProperty] private Property ID, dataType, output;
    [JsonProperty] private NetworkReceivableProps netProps;
    [JsonProperty] private Property output;
    //the constructor needs to have a paramater so that the deserializer can use the default one
    public NetReceiveNode(ColorVec pos) : base(pos)
    {
        base.nodeDisc = "Net Receive";
        netProps = new NetworkReceivableProps(this);
        output = base.CreateOutputProperty("output");
    }

    public override void Init()
    {
        base.Init();
        netProps.init();
        netProps.RegisterNetReceive(this);
    }

    public void ReceiveData(NetworkMessage message)
    {
        //string value = dataType.GetData().ToString();
        switch ((NetworkMessage.DataType)netProps.dataType.GetData())
        {
            case NetworkMessage.DataType.Float:
                output.Invoke(new EvaluableFloat(ByteConverter.GetFLoat(message.data)));
                break;
            case NetworkMessage.DataType.ByteArray:
                output.Invoke(ByteConverter.GetColorTable(message.data));
                break;
            default:
                Debug.LogWarning("Invalid data type received with ID of " + message.ID);
                break;
        }
    }
}
