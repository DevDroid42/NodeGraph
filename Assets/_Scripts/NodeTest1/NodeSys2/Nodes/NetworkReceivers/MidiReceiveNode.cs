using Newtonsoft.Json;
using nodeSys2;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MidiReceiveNode : Node, INetReceivable
{
    [JsonProperty] private NetworkReceivableProps netReceiveProps;
    [JsonProperty] private MidiProperties midiProps;
    [JsonProperty] private Property tableOutput, positionOuput, pressedTrigger;

    //used to generate the color table
    private byte[] byteBuffer;
    int low, high, size;
    public MidiReceiveNode(ColorVec pos) : base(pos)
    {
        base.nodeDisc = "Midi Receive";
        netReceiveProps = new NetworkReceivableProps(this);
        netReceiveProps.dataType.visible = false;
        netReceiveProps.dataType.SetData(NetworkMessage.DataType.ByteArray);
        midiProps = new MidiProperties(this);
        tableOutput = CreateOutputProperty("Table Output");
        positionOuput = CreateOutputProperty("Position Output");
        pressedTrigger = CreateOutputProperty("Pressed Trigger");
    }

    public override void Init()
    {
        base.Init();
        netReceiveProps.init();
        netReceiveProps.RegisterNetReceive(this);
        midiProps.notePressedCallback = HandlePressed;
    }

    public void ReceiveData(NetworkMessage message)
    {
        byte[] incoming = message.data;
        midiProps.UpdateState(incoming);
        low = Mathf.Max((int)midiProps.lowerBound.GetEvaluable().EvaluateValue(), 0);
        high = Mathf.Min((int)midiProps.upperBound.GetEvaluable().EvaluateValue(), incoming.Length);
        size = Mathf.Max(high - low, 0);
        if(byteBuffer == null || byteBuffer.Length != size)
        {
            byteBuffer = new byte[size];
        }
        for (int i = low; i < high; i++)
        {
            byteBuffer[i-low] = incoming[i];
        }
        tableOutput.Invoke(ByteConverter.GetColorTable(byteBuffer));
    }

    private void HandlePressed(List<int> pressed)
    {
        int highestPressed = -1;
        for (int i = 0; i < pressed.Count; i++)
        {
            if(pressed[i] > low && pressed[i] < high)
            {
                highestPressed = pressed[i];
            }
        }
        if(highestPressed != -1)
        {
            float position = (((float)highestPressed-low) / ((float)high-low));
            positionOuput.Invoke(new EvaluableFloat(position));
            pressedTrigger.Invoke(new Pulse());
        }
        
    }

    
}