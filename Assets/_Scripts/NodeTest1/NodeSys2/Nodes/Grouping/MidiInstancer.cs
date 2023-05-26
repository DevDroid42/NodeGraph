using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using nodeSys2;

public class MidiInstancer : StaticInstancer, INetReceivable
{
    [JsonProperty] private NetworkReceivableProps netReceiveProps;
    [JsonProperty] private MidiProperties midiProps;
    private int currentInstance;
    private int low, high, size;
    private float lastPos;
    private byte[] incoming;

    public MidiInstancer(ColorVec pos) : base(pos)
    {
        base.nodeDisc = "Midi Instancer";
        netReceiveProps = new NetworkReceivableProps(this);
        netReceiveProps.dataType.visible = false;
        netReceiveProps.dataType.SetData(NetworkMessage.DataType.ByteArray);
        midiProps = new MidiProperties(this);
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
        Handle();
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
                    groups[currentInstance].PublishToGraph(prop.ID, prop.GetData());
                }
                float velocity = incoming[pressed[i]] / 255f;
                float position = (pressed[i] - low) / (float)(high - low);
                float deltaPos = position - lastPos;
                lastPos = position;
                groups[currentInstance].PublishToGraph(MidiInfoNode.velocityKey, new EvaluableFloat(velocity));
                groups[currentInstance].PublishToGraph(MidiInfoNode.posKey, new EvaluableFloat(position));
                groups[currentInstance].PublishToGraph(MidiInfoNode.deltaPosKey, new EvaluableFloat(deltaPos));
                groups[currentInstance].PulseGraph();
                currentInstance = (currentInstance + 1) % groups.Count;
            }
        }
        pressed = null;
    }

    public void ReceiveData(NetworkMessage message)
    {
        incoming = message.data;
        low = Mathf.Max((int)midiProps.lowerBound.GetEvaluable().EvaluateValue(), 0);
        high = Mathf.Min((int)midiProps.upperBound.GetEvaluable().EvaluateValue(), incoming.Length);
        size = Mathf.Max(high - low, 0);
        midiProps.UpdateState(incoming);
    }
}
