using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using nodeSys2;

public class MidiInstancer : StaticInstancer, INetReceivable
{
    [JsonProperty] private NetworkReceivableProps netReceiveProps;
    [JsonProperty] private MidiProperties midiProps;
    [JsonProperty] private Property output;
    private int currentInstance;

    public MidiInstancer(ColorVec pos) : base(pos)
    {
        base.nodeDisc = "Midi Receive";
        netReceiveProps = new NetworkReceivableProps(this);
        netReceiveProps.dataType.visible = false;
        netReceiveProps.dataType.SetData(NetworkMessage.DataType.ByteArray);
        midiProps = new MidiProperties(this);
        output = CreateOutputProperty("Output");
    }

    public override void Init()
    {
        base.Init();
        netReceiveProps.init();
        netReceiveProps.RegisterNetReceive(this);
        midiProps.notePressedCallback = HandlePressed;
        currentInstance = 0;
    }

    private List<int> pressed;
    private void HandlePressed(List<int> pressed)
    {
        this.pressed = pressed;
    }

    public override void Handle()
    {
        if(pressed != null)
        {
            for (int i = 0; i < pressed.Count; i++)
            {
                //Debug.Log("InstanceRunning: " + currentInstance + "\t Group Count:" + groups.Count);
                foreach (Property prop in groupInputs)
                {
                    if (groups[currentInstance] != null)
                    {
                        groups[currentInstance].PublishToGraph(prop.ID, prop.GetData());
                    }
                }
                groups[currentInstance].PulseGraph();
                currentInstance = (currentInstance + 1) % groups.Count;
            }
        }
        pressed = null;
    }

    public void ReceiveData(NetworkMessage message)
    {
        byte[] incoming = message.data;
        midiProps.UpdateState(incoming);
    }
}
