using Newtonsoft.Json;
using nodeSys2;
using System;
using System.Text;
using UnityEngine;
//using UnityEngine;

public class NetReceiveNode : Node
{

    [JsonProperty] private Property ID, dataType, output;
    //the constructor needs to have a paramater so that the deserializer can use the default one
    public NetReceiveNode(ColorVec pos) : base(pos)
    {
        base.nodeDisc = "Net Receive";
        dataType = base.CreateInputProperty("Data Type", false, new NetworkMessage.DataType());
        dataType.interactable = true;
        ID = base.CreateInputProperty("Data ID", false, new StringData(""));
        ID.interactable = true;
        output = base.CreateOutputProperty("output");
    }

    public override void Init()
    {
        base.Init();
        Graph.nodeCollection.RegisterNetReceiveNode(ID.GetData().ToString(), dataType.GetData().ToString(), this);
        ProccessEnums();
    }

    public void ReceiveData(NetworkMessage message)
    {
        //string value = dataType.GetData().ToString();
        switch ((NetworkMessage.DataType)dataType.GetData())
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

    private void ProccessEnums()
    {
        if (dataType.GetData().GetType() == typeof(string))
        {
            dataType.SetData(Enum.Parse(typeof(NetworkMessage.DataType), (string)dataType.GetData()));
        }
    }

}
