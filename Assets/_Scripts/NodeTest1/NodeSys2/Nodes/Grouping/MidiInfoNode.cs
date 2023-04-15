using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using nodeSys2;

public class MidiInfoNode : Node
{
    [JsonIgnore]
    public float position, deltaPos, velocity;
    [JsonProperty] private Property positonProp, deltaPosProp, velocityProp;

    public MidiInstancer(ColorVec pos) : base(pos)
    {
        base.nodeDisc = "Midi Info";
        positonProp = CreateOutputProperty("position");
        deltaPosProp = CreateOutputProperty("delta position");
        velocityProp = CreateOutputProperty("velocity");
    }

    public override void Handle()
    {
        positonProp.Invoke(new EvaluableFloat(position));
        deltaPosProp.Invoke(new EvaluableFloat(deltaPos));
        velocityProp.Invoke(new EvaluableFloat(velocity));
    }
}
