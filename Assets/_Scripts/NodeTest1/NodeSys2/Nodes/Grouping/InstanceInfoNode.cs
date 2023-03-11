using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using nodeSys2;

public class InstanceInfoNode : Node
{
    [JsonIgnore]
    public float index, count, ratio;
    [JsonProperty] private Property indexProp, countProp, ratioProp;
    
    public InstanceInfoNode(ColorVec pos) : base(pos)
    {
        base.nodeDisc = "Instance Info";
        indexProp = CreateOutputProperty("Instance Index");
        countProp = CreateOutputProperty("Total Instance Count");
        ratioProp = CreateOutputProperty("Instance Ratio");
    }

    public override void Handle()
    {
        Debug.Log("index: " + index + " count: " + count + " ratio: " + ratio);
        indexProp.Invoke(new EvaluableFloat(index));
        countProp.Invoke(new EvaluableFloat(count));
        ratioProp.Invoke(new EvaluableFloat(ratio));
    }
}
