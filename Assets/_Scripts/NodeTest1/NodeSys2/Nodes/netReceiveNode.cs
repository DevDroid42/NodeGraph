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
        base.nodeDisc = "NodeNet Receive";
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
        //to string will convert to the int representations of the enum
        switch (dataType.GetData().ToString())
        {
            case "0"://Value
            case "Value":
                output.Invoke(new EvaluableFloat(ByteConverter.GetInt(message.data)));
                break;
            default:
                Debug.LogWarning("Invalid data type received with ID of " + message.ID);
                break;
        }
        if (message.ID == ((StringData)ID.GetData()).txt)
        {
            //Debug.Log("Message ID match found: " + message);
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
