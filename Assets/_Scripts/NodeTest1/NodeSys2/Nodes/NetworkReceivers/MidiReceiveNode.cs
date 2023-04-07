using Newtonsoft.Json;
using nodeSys2;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MidiReceiveNode : Node, INetReceivable
{
    [JsonProperty] private MidiProperties midiProps;
    public MidiReceiveNode()
    {
        midiProps = new MidiProperties(this);
    }

    public void ReceiveData(NetworkMessage message)
    {
        throw new System.NotImplementedException();
    }
}