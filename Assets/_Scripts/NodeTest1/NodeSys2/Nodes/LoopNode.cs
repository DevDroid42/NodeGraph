using UnityEngine;
using nodeSys2;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;

public class LoopNode : Node
{
    public Property ResetTrig, InvertTrig, stepTrig, stepSize, loopTypeProp, minP, maxP, rateP, output, startP;
    private float start, min, max, rate, current;

    [JsonConverter(typeof(StringEnumConverter))]
    public enum LoopType {loop, stop, invert}
    private LoopType loopType;
    private int rateInverter = 1;

    public LoopNode(bool x)
    {
        nodeDisc = "Loop";
        ResetTrig = CreateInputProperty("Reset Trigger", true, new Pulse(false), typeof(Pulse));
        ResetTrig.visible = false;
        InvertTrig = CreateInputProperty("Invert Trigger", true, new Pulse(false), typeof(Pulse));
        InvertTrig.visible = false;
        stepTrig = CreateInputProperty("Step Trigger", true, new Pulse(false), typeof(Pulse));
        stepTrig.visible = false;
        stepSize = CreateInputProperty("Step Size", true, new EvaluableFloat(0.1f));
        stepSize.visible = false;
        stepSize.interactable = true;
        loopTypeProp = CreateInputProperty("Loop type", false, new LoopType());
        loopTypeProp.interactable = true;
        startP = CreateInputProperty("Start", true, new EvaluableFloat(0));
        startP.interactable = true;
        minP = CreateInputProperty("min", true, new EvaluableFloat(0));
        minP.interactable = true;
        maxP = CreateInputProperty("Max", true, new EvaluableFloat(1));
        maxP.interactable = true;
        rateP = CreateInputProperty("rate", true, new EvaluableFloat(1f));
        rateP.interactable = true;
        output = CreateOutputProperty("output");
    }

    public override void Init()
    {
        processData();
        ProcessEnums();
        rateInverter = 1;
        frameDelagate -= Frame;
        frameDelagate += Frame;
        current = start;
    }

    public override void Handle()
    {
        processData();
        if (((Pulse)ResetTrig.GetData()).PulsePresent())
        {
            current = start;
        }
        if (((Pulse)InvertTrig.GetData()).PulsePresent())
        {
            rateInverter *= -1;
        }
    }
    
    public override void Frame(float deltaTime)
    {        
        switch (loopType)
        {
            case LoopType.loop:
                Increment(deltaTime);
                if (current - max > 0)
                {
                    current = min + (current - max);
                }
                if (min - current > 0)
                {
                    current = max - (min - current);
                }
                break;
            case LoopType.stop:
                if (((Pulse)stepTrig.GetData()).PulsePresent())
                {
                    current += ((Evaluable)stepSize.GetData()).EvaluateValue(0) * rateInverter;
                }
                if (current <= min)
                {
                    current = min;
                    if (rate * rateInverter > 0)
                    {                        
                        Increment(deltaTime);
                    }
                }
                else if (current >= max)
                {
                    current = max;
                    if (rate * rateInverter < 0)
                    {                        
                        Increment(deltaTime);
                    }
                }
                else
                {
                    Increment(deltaTime);
                }
                break;
            case LoopType.invert:
                Increment(deltaTime);
                if (current < min)
                {
                    current = min;
                    rateInverter *= -1;
                }
                else if (current > max)
                {
                    current = max;
                    rateInverter *= -1;
                }
                break;
        }

        output.Invoke(new EvaluableFloat(current));
    }

    private void Increment(float deltaTime)
    {
        if (((Pulse)stepTrig.GetData()).PulsePresent())
        {
            current += ((Evaluable)stepSize.GetData()).EvaluateValue(0) * rateInverter;
        }
        current += rate * rateInverter * deltaTime;
    }

    private void processData()
    {
        Evaluable data = new Evaluable();
        if (minP.TryGetDataType(ref data))
        {
            min = data.EvaluateValue(0);
        }
        if (maxP.TryGetDataType(ref data))
        {
            max = data.EvaluateValue(0);
        }
        if (rateP.TryGetDataType(ref data))
        {
            rate = data.EvaluateValue(0);
        }
        if (startP.TryGetDataType(ref data))
        {
            start = data.EvaluateValue(0);
        }
    }

    private void ProcessEnums()
    {
        if (loopTypeProp.GetData().GetType() == typeof(string))
        {   
            loopTypeProp.SetData((LoopType)Enum.Parse(typeof(LoopType), (string)loopTypeProp.GetData()));
        }
        loopType = ((LoopType)loopTypeProp.GetData());
    }


}

public class AdvancedLoopNode : LoopNode
{
    public AdvancedLoopNode(bool x) : base(x)
    {
        nodeDisc = "Advanced Loop";
        ResetTrig.visible = true;
        InvertTrig.visible = true;
        stepTrig.visible = true;
        stepSize.visible = true;
    }
}
