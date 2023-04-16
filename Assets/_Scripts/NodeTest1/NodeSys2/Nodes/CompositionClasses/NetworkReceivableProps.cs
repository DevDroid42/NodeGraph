using Newtonsoft.Json;
using nodeSys2;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkReceivableProps
{
    public Property ID, dataType;
    public NetworkReceivableProps(Node parentNode)
    {
        dataType = parentNode.CreateInputProperty("Data Type", false, new NetworkMessage.DataType());
        dataType.interactable = true;
        ID = parentNode.CreateInputProperty("Data ID", false, new StringData(""));
        ID.interactable = true;
    }

    [JsonConstructor] public NetworkReceivableProps() { }

    public void init()
    {
        EnumUtils.ConvertEnum<NetworkMessage.DataType>(dataType);
    }

    public void RegisterNetReceive(INetReceivable parentNode)
    {
        Graph.globalNodeCollection.RegisterNetReceiveNode(ID.GetData().ToString(), dataType.GetData().ToString(), parentNode);
    }
}
