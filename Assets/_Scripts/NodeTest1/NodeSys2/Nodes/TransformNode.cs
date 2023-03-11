using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using nodeSys2;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json;

public class TransformNode : Node
{
    [JsonProperty] private Property inputData, localOffset, scale, pivot, rot, globalOffset, output;
    [JsonProperty] private EvaluableTransform evaluableTransform = new EvaluableTransform();

    public TransformNode(ColorVec pos) : base(pos)
    {
        base.nodeDisc = "Transform";
        inputData = CreateInputProperty("Input", true, new EvaluableBlank());
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
        ManipulateTransform();
    }

    public override void Handle()
    {
        base.Handle();
        ManipulateTransform();
        evaluableTransform.child = ((IEvaluable)inputData.GetData());
        output.Invoke(evaluableTransform);
    }

    private void ManipulateTransform()
    {
        evaluableTransform.localOffset = ((IEvaluable)(localOffset.GetData())).EvaluateValue(0);
        evaluableTransform.scale = ((IEvaluable)(scale.GetData())).EvaluateValue(0);
        evaluableTransform.pivot = ((IEvaluable)(pivot.GetData())).EvaluateValue(0);
        evaluableTransform.globalOffset = ((IEvaluable)(globalOffset.GetData())).EvaluateValue(0);
    }
}
