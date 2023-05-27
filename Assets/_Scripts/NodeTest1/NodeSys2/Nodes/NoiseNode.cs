using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using nodeSys2;
using Newtonsoft.Json;

public class NoiseNode : Node
{
    [JsonProperty] Property scale, offsetx, offsety, output;
    private EvaluableNoise noise;

    public NoiseNode(ColorVec pos) : base(pos)
    {
        nodeDisc = "Noise";
        scale = CreateInputProperty("Scale", true, new EvaluableFloat(1));
        scale.interactable = true;
        offsetx = CreateInputProperty("Offset X", true, new EvaluableFloat(1));
        offsetx.interactable = true;
        offsety = CreateInputProperty("Offset Y", true, new EvaluableFloat(1));
        offsety.interactable = true;
        output = CreateOutputProperty("Output");
    }

    private void UpdateNoise()
    {
        noise.scale = scale.GetEvaluable().EvaluateValue();
        noise.offsetx = offsetx.GetEvaluable().EvaluateValue();
        noise.offsety = offsety.GetEvaluable().EvaluateValue();
    }

    public override void Init()
    {
        noise = new EvaluableNoise();
        UpdateNoise();
    }

    public override void Init2()
    {
        output.Invoke(noise);
    }

    public override void Handle()
    {
        UpdateNoise();
        output.Invoke(noise);
    }
}
