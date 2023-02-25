using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using nodeSys2;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json;

public class TransformNode : Node
{
    public Property setTypeProp, inputData, localOffset, scale, pivot, rot, globalOffset, output;

    [JsonConverter(typeof(StringEnumConverter))]
    public enum SetType {OverWrite, Add, Subtract}

    public TransformNode(ColorVec pos) : base(pos)
    {
        base.nodeDisc = "Transform";
        setTypeProp = CreateInputProperty("Mix Type", false, new SetType());
        setTypeProp.interactable = true;
        inputData = CreateInputProperty("Input", true, new Evaluable());
        localOffset = CreateInputProperty("Local Offset", true, new EvaluableFloat(0));
        localOffset.interactable = true;
        scale = CreateInputProperty("Scale", true, new EvaluableFloat(1));
        scale.interactable = true;
        pivot = CreateInputProperty("pivot", true, new EvaluableFloat(0));
        pivot.interactable = true;
        rot = CreateInputProperty("Rotation", true, new EvaluableFloat(0));
        rot.interactable = true;
        globalOffset = CreateInputProperty("Global Offset", true, new EvaluableFloat(0));
        globalOffset.interactable = true;
        output = CreateOutputProperty("Output");
    }

    public override void Init()
    {
        base.Init();
        EnumUtils.ConvertEnum<SetType>(setTypeProp);
        ManipulateTransform();
    }

    public override void Handle()
    {
        base.Handle();
        ManipulateTransform();
        output.Invoke(((Evaluable)inputData.GetData()));
    }

    private void ManipulateTransform()
    {
        Evaluable data = (Evaluable)inputData.GetData();        
        switch ((SetType)setTypeProp.GetData())
        {
            case SetType.OverWrite:
                data.localOffset = ((Evaluable)(localOffset.GetData())).EvaluateValue(0);
                data.scale = ((Evaluable)(scale.GetData())).EvaluateValue(0);
                data.pivot = ((Evaluable)(pivot.GetData())).EvaluateValue(0);
                data.rot = ((Evaluable)(rot.GetData())).EvaluateValue(0);
                data.globalOffset = ((Evaluable)(globalOffset.GetData())).EvaluateValue(0);
                break;
            case SetType.Add:
                data.localOffset += ((Evaluable)(localOffset.GetData())).EvaluateValue(0);
                data.scale += ((Evaluable)(scale.GetData())).EvaluateValue(0);
                data.pivot += ((Evaluable)(pivot.GetData())).EvaluateValue(0);
                data.rot += ((Evaluable)(rot.GetData())).EvaluateValue(0);
                data.globalOffset += ((Evaluable)(globalOffset.GetData())).EvaluateValue(0);
                break;
            case SetType.Subtract:
                data.localOffset -= ((Evaluable)(localOffset.GetData())).localOffset;
                data.scale -= ((Evaluable)(scale.GetData())).scale;
                data.pivot -= ((Evaluable)(pivot.GetData())).pivot;
                data.rot -= ((Evaluable)(rot.GetData())).rot;
                data.globalOffset -= ((Evaluable)(globalOffset.GetData())).globalOffset;
                break;
            default:
                break;
        }
    }
}
