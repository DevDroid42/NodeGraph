using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using nodeSys2;

//public float position, deltaPos, velocity;
public class MidiInfoNode : InfoNode
{
    public static readonly string 
        posKey = "Info_position",
        deltaPosKey = "Info_deltaPos",
        velocityKey = "Info_velocity";

    [JsonProperty] private Property positionProp, deltaPosProp, velocityProp;
    [JsonProperty] private Property positionPropOut, deltaPosPropOut, velocityPropOut;

    public MidiInfoNode(ColorVec pos) : base(pos)
    {
        base.nodeDisc = "Midi Info";
        positionProp = CreateInputProperty(posKey, false, new EvaluableFloat(0));
        positionProp.visible = false;
        RegisterInfoInputProperty(positionProp);
        positionPropOut = CreateOutputProperty("Note Position");

        deltaPosProp = CreateInputProperty(deltaPosKey, false, new EvaluableFloat(0));
        deltaPosProp.visible = false;
        RegisterInfoInputProperty(deltaPosProp);
        deltaPosPropOut = CreateOutputProperty("Note Delta Position");

        velocityProp = CreateInputProperty(velocityKey, false, new EvaluableFloat(0));
        velocityProp.visible = false;
        RegisterInfoInputProperty(velocityProp);
        velocityPropOut = CreateOutputProperty("Note Velocity");
    }

    public override void Handle()
    {
        positionPropOut.Invoke(positionProp.GetData());
        deltaPosPropOut.Invoke(deltaPosProp.GetData());
        velocityPropOut.Invoke(velocityProp.GetData());
    }
}
