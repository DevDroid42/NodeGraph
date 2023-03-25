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

    [JsonProperty] private Property input, recordingTrigger, armed, recording, name, resolution;

    public RecordingNode(ColorVec pos) : base(pos)
    {
        base.nodeDisc = "Recorder";
        input = CreateInputProperty("input", true, new EvaluableBlank());
        recording = CreateInputProperty("Recording", false, new EvaluableBool(false));
        name = CreateInputProperty("name", false, new StringData(""));
        name.interactable = true;
        resolution = CreateInputProperty("resolution", false, new EvaluableFloat(256));
        resolution.interactable = true;
        recordingTrigger = CreateInputProperty("Record Trigger", true, new Pulse(false));
        armed = CreateInputProperty("Armed" , true, new EvaluableBool(true));
        armed.interactable = true;
        
    }

    public override void Init()
    {
        base.Init();
        RegisterFrameMethod(Frame);
        Graph.nodeCollection.RegisterRecordingNode(this);
    }

    public void SetRecording(bool state)
    {
        if (state)
        {
            if (((IEvaluable)armed.GetData()).EvaluateValue(0) > 0.5)
            {
                Recording = true;
            }
        }else
        {
            Recording = false;
        }
        recording.SetData(new EvaluableBool(Recording));
    }

    float time = 0;
    public override void Frame(float deltaTime)
    {
        //recordingState.Setval(Recording);
        time += deltaTime;
        if (Recording && time > 0.020)
        {
            time = 0;
            int res = (int)(((IEvaluable)resolution.GetData()).EvaluateValue(0));
            Graph.recorder.RecordFrame(
                name.GetData().ToString(),
                BatchEvaluation.EvaluateColorRange((IEvaluable)input.GetData(), res)
            );
        }
    }

    public override void Handle()
    {
        if (((Pulse)recordingTrigger.GetData()).PulsePresent())
        {
            Recording = !Recording;
        }
    }
}
