using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using nodeSys2;
using Newtonsoft.Json;

public class PointNode : Node
{
    [JsonProperty] Property sizeProp, positionProp, pointColorProp, backgroundColorProp, interpolationEnabledProp, outputProp;
    private EvaluablePoint point;
    private float size, pos;

    public PointNode(ColorVec pos) : base(pos)
    {
        nodeDisc = "Point";
        sizeProp = CreateInputProperty("Size", true, new EvaluableFloat(0.1f));
        sizeProp.interactable = true;
        positionProp = CreateInputProperty("Position", true, new EvaluableFloat(0.5f));
        positionProp.interactable = true;
        pointColorProp = CreateInputProperty("Point Color", true, new EvaluableColorVec(new ColorVec(1)));
        pointColorProp.interactable = true;
        backgroundColorProp = CreateInputProperty("Background Color", true, new EvaluableColorVec(new ColorVec(0)));
        backgroundColorProp.interactable = true;
        interpolationEnabledProp = CreateInputProperty("Interpolation", true, new EvaluableBool(true));
        interpolationEnabledProp.interactable = true;
        outputProp = CreateOutputProperty("Output");
    }

    public override void Init()
    {
        point = new EvaluablePoint();
        size = sizeProp.GetEvaluable().EvaluateValue();
        pos = positionProp.GetEvaluable().EvaluateValue();
    }

    public override void Init2()
    {
        RenderPoint();
        outputProp.Invoke(point);
    }

    public override void Handle()
    {
        RenderPoint();
        outputProp.Invoke(point);
    }

    private void RenderPoint()
    {
        point.backgroundColor = backgroundColorProp.GetEvaluable();
        point.pointColor = pointColorProp.GetEvaluable();
        size = sizeProp.GetEvaluable().EvaluateValue();
        float newPos = positionProp.GetEvaluable().EvaluateValue();
        float deltaPos = newPos - pos;

        point.start = newPos - (size / 2.0f);
        point.end = newPos + (size / 2.0f);
        if (interpolationEnabledProp.GetEvaluable().EvaluateValue() >= 0.5f)
        {
            if (deltaPos > 0)
            {
                point.start = pos - (size / 2.0f);
                point.end = newPos + (size / 2.0f);
            }
            else if(deltaPos < 0)
            {
                point.start = newPos - (size / 2.0f);
                point.end = pos + (size / 2.0f);
            }
        }
        pos = newPos;
    }

}
