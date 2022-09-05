using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using nodeSys2;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;

public class GroupInputNode : Node, INameable
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum InputType
    {
        Static, Instanced
    }    
    public Property input, inputType, name, output;

    public GroupInputNode(ColorVec pos) : base(pos)
    {
        base.nodeDisc = "Group Input";
        input = CreateInputProperty("DataIn", false, new Evaluable(), typeof(ICopyable));
        input.visible = false;
        inputType = CreateInputProperty("Input Type", false, new InputType());
        inputType.interactable = true;
        name = CreateInputProperty("Data tag", false, new StringData("input"), typeof(StringData));
        name.interactable = true;
        output = CreateOutputProperty("output");
    }

    public override void Init()
    {
        base.Init();
        ProccessEnums();
    }

    public override void Handle()
    {
        output.Invoke(input.GetData());
    }
    public string getName()
    {
        return ((StringData)name.GetData()).txt;
    }

    private void ProccessEnums()
    {
        if(inputType.GetData().GetType() == typeof(string))
        {
            inputType.SetData(Enum.Parse(typeof(InputType), (string)inputType.GetData()));
        }
    }
}
