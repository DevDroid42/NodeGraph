using UnityEngine;
using nodeSys2;

public class LoopNode : Node
{
    public Property minP, maxP, rateP, output, startP;
    private float start, min, max, rate, current;
    public LoopNode(bool x)
    {
        nodeDisc = "Loop";

        startP = CreateInputProperty("Start", true, new EvaluableFloat(0), typeof(Evaluable));
        startP.interactable = true;
        minP = CreateInputProperty("min", true, new EvaluableFloat(0), typeof(Evaluable));
        minP.interactable = true;
        maxP = CreateInputProperty("Max", true, new EvaluableFloat(1), typeof(Evaluable));
        maxP.interactable = true;
        rateP = CreateInputProperty("rate", true, new EvaluableFloat(0.1f), typeof(Evaluable));
        rateP.interactable = true;
        output = CreateOutputProperty("output");
    }

    public override void Init()
    {
        processData();
        frameDelagate -= Frame;
        frameDelagate += Frame;
        current = start;
    }

    public override void Handle()
    {
        processData();
    }

    public override void Frame(float deltaTime)
    {
        current += rate * deltaTime;
        if(current < min)
        {
            current = max;
        }else if(current > max)
        {
            current = min;
        }
        output.Invoke(new EvaluableFloat(current));
        output.disc = "output: " + current;
    }

    private void processData()
    {
        Evaluable data = new Evaluable();
        if (minP.TryGetDataType(ref data))
        {
            min = data.EvaluateValue(0, 0, 0, 0);
        }
        if (maxP.TryGetDataType(ref data))
        {
            max = data.EvaluateValue(0, 0, 0, 0);
        }
        if (rateP.TryGetDataType(ref data))
        {
            rate = data.EvaluateValue(0, 0, 0, 0);
        }
        if (startP.TryGetDataType(ref data))
        {
            start = data.EvaluateValue(0, 0, 0, 0);
        }
    }




}
