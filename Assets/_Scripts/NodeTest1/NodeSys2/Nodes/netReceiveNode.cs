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
        ProccessEnums();
        nodeNetDelagate -= ReceiveData;
        nodeNetDelagate += ReceiveData;
    }

    public override void ReceiveData(NetworkMessage message)
    {
        if (message.ID == ((StringData)ID.GetData()).txt && message.dataType == (NetworkMessage.DataType)dataType.GetData())
        {
            //string value = dataType.GetData().ToString();
            switch ((NetworkMessage.DataType)dataType.GetData())
            {
                case NetworkMessage.DataType.Float:
                    output.Invoke(new EvaluableFloat(ByteConverter.GetFLoat(message.data)));
                    break;
                default:
                    Debug.LogWarning("Invalid data type received with ID of " + message.ID);
                    break;
            }


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
