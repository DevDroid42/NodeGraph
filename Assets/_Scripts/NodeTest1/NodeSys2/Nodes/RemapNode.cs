using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using nodeSys2;
using Newtonsoft.Json;

public class RemapNode : Node
{
    [JsonProperty] private Property input, map, output;
    [JsonProperty] private EvaluableRemap remaper = new EvaluableRemap();

    public RemapNode(ColorVec pos) : base(pos)
    {
        base.nodeDisc = "Transform";
        input = CreateInputProperty("input", true, new EvaluableBlank());
        input.interactable = true;
        input.internalRepresentation = EditorTypeManagement.Editor.table;
        map = CreateInputProperty("map", true, new EvaluableBlank());
        map.interactable = true;
        map.internalRepresentation = EditorTypeManagement.Editor.table;
        output = CreateOutputProperty("Output");
    }

    public override void Init()
    {
        base.Init();
        UpdateRemaper();
    }

    public override void Handle()
    {
        base.Handle();
        UpdateRemaper();
        output.Invoke(remaper);
    }

    private void UpdateRemaper()
    {
        remaper.input = input.GetEvaluable();
        remaper.map = map.GetEvaluable();
    }
}
