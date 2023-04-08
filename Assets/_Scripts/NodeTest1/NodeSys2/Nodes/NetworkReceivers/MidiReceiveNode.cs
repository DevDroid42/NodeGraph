using Newtonsoft.Json;
using nodeSys2;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MidiReceiveNode : Node, INetReceivable
{
    [JsonProperty] private NetworkReceivableProps netReceiveProps;
    [JsonProperty] private MidiProperties midiProps;
    public MidiReceiveNode()
    {
        netReceiveProps = new NetworkReceivableProps(this);
        midiProps = new MidiProperties(this);
    }

    public override void Init()
    {
        base.Init();
        netReceiveProps.init();
        netReceiveProps.RegisterNetReceive(this);
    }

    public void ReceiveData(NetworkMessage message)
    {
        
    }
}