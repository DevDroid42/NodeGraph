using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using nodeSys2;
using Newtonsoft.Json;

public class RecordingNode : Node
{
    public bool Recording
    {
        get; private set;
    }

    [JsonProperty] private Property recordingTrigger, armed, name, resolution;

    public RecordingNode(ColorVec pos) : base(pos)
    {
        base.nodeDisc = "Recorder";
        name = CreateInputProperty("name", false, new StringData(""));
        name.interactable = true;
        resolution = CreateInputProperty("resolution", false, new EvaluableFloat(128));
        resolution.interactable = true;
        recordingTrigger = CreateInputProperty("Record Trigger", true, new Pulse(false));
        armed = CreateInputProperty("Armed" , true, new EvaluableBool(false));
        armed.interactable = true;
    }

    public override void Init()
    {
        base.Init();
        Node.RegisterFrameMethod(Frame);
        Graph.nodeCollection.RegisterRecordingNode(this);
    }

    public override void Frame(float deltaTime)
    {
        base.Frame(deltaTime);
    }

    public override void Handle()
    {
        if (((Pulse)recordingTrigger.GetData()).PulsePresent())
        {
            
        }
    }
}
