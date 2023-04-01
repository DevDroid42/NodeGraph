using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using nodeSys2;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

public class FollowNode : Node
{
    [JsonProperty] Property input, followAlgorithm, followRate, snapUp, snapDown, resolution, output;
    [JsonConverter(typeof(StringEnumConverter))]
    public enum FollowAlgorithm
    {
        logarithmic, gravity
    }
    private EvaluableColorTable outputTable;
    //current colors
    private ColorVec[] currentColors;

    public FollowNode(ColorVec pos) : base(pos)
    {
        base.nodeDisc = "Follow";
        input = CreateInputProperty("input", true, new EvaluableBlank());
        input.internalRepresentation = EditorTypeManagement.Editor.table;
        input.interactable = true;
        resolution = CreateInputProperty("Resolution", false, new EvaluableFloat(1));
        resolution.interactable = true;
        followAlgorithm = CreateInputProperty("Follow Algorithm", false, new FollowAlgorithm());
        followAlgorithm.interactable = true;
        followRate = CreateInputProperty("Follow Rate", false, new EvaluableFloat(1));
        followRate.interactable = true;
        snapUp = CreateInputProperty("Snap Up", true, new EvaluableBool(true));
        snapUp.interactable = true;
        snapDown = CreateInputProperty("Snap Down", true, new EvaluableBool(false));
        snapDown.interactable = true;
        output = CreateOutputProperty("output");
    }

    public override void Init()
    {
        RegisterFrameMethod(Frame);
        int res = (int)resolution.GetEvaluable().EvaluateValue();
        if (res < 0)
        {
            res = 0;
        }
        outputTable = new EvaluableColorTable(res);
        currentColors = new ColorVec[res];
    }

    public override void Frame(float deltaTime)
    {
        int count = currentColors.Length;
        float rate = Mathf.Clamp(followRate.GetEvaluable().EvaluateValue(), 0, 1);
        for (int i = 0; i < currentColors.Length; i++)
        {
            float position = (count == 1) ? 0 : (float)i / (count - 1);
            ColorVec targetColor = input.GetEvaluable().EvaluateColor(position);
            if (snapUp.GetEvaluable().EvaluateValue() >= 0.5 && ((float)targetColor) > ((float)currentColors[i])
            || snapDown.GetEvaluable().EvaluateValue() >= 0.5 && ((float)targetColor) < ((float)currentColors[i]))
            {
                currentColors[i] = targetColor;
            }
            else
            {
                currentColors[i] = ColorOperations.lerp(currentColors[i], targetColor, rate);
            }
            outputTable.SetKey(i, currentColors[i]);
        }
        output.Invoke(outputTable);
    }
}
