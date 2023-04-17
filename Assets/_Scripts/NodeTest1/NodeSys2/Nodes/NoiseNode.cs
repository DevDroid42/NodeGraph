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
        scale = CreateInputProperty("Scale", true, new EvaluableFloat(1));
        scale.interactable = true;
        offsetx = CreateInputProperty("Offset X", true, new EvaluableFloat(1));
        offsetx.interactable = true;
        offsety = CreateInputProperty("Offset Y", true, new EvaluableFloat(1));
        offsety.interactable = true;
    }
}
