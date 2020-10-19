using UnityEngine;
using nodeSys2;

public class LoopNode : Node
{
    FloatData loopNum = new FloatData(0);    
    public LoopNode()
    {
        /**
        nodeDisc = "Loop Node";
        //3 constants 1 viewables
        base.SetupConstantsViewables(3,1);
        //value to loop back to when passing max
        SetConstant(0, new IntData(0), "min");
        //point to loop back to min
        SetConstant(1, new IntData(1), "max");
        //how much to add per second
        SetConstant(2, new FloatData(0.1f), "RatePerSecond");
        InitPorts(0, 1);
        **/
    }

    public override void Init()
    {      
        /**
        //let user see the current number. This needs to be run here because 
        //references to primitive data types aren't serialized. 
        SetViewable(0, loopNum, "CurrentOutput");

        //removing does nothing if it's not already assigned so remove first incase it's already there to prevent dulplicates
        frameDelagate -= Frame;
        frameDelagate += Frame;
        **/
    }

    public override void Frame(float deltaTime)
    {
        /**
        loopNum.num += ((FloatData)constants[2]).num * Time.deltaTime;
        if(loopNum.num > ((IntData)constants[1]).num)
        {
            loopNum.num = ((IntData)constants[0]).num;
        }
        outputs[0].Invoke(loopNum);
        **/
    }


}
