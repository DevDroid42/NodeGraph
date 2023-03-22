using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using nodeSys2;
using Newtonsoft.Json;

public class RecordingNode : Node
{
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
}
