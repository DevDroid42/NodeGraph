using UnityEngine;
public class Evaluable : ICopyable
{
    //offset applied pre-scaling
    public float localOffset = 0;
    //offset applied post-scaling
    public float globalOffset = 0;
    public float scale = 1;
    public float rot = 0;
    public float pivot = 0;    

    //Note: should always return a new color, not one with a reference inside the evaluable object
    public virtual ColorVec EvaluateColor(float vector)
    {
        return new ColorVec(0);
    }

    public virtual float EvaluateValue(float vector)
    {
        return 0;
    }

    //used to get the resolution of the Evaluable data type.
    public virtual ColorVec GetResolution()
    { 
        return 0;
    }

    //used to get copy of Evaluable object so references arn't being passed between nodes. Reference passing would entangle
    //node states and create problems
    public virtual object GetCopy()
    {
        //Debug.LogWarning("WARNING: Using Default get copy, this should never be run");
        return new Evaluable();
    }

    //to be used for vector transformations in subclasses
    protected float TransformVector(float input)
    {
        //avoid divide by zero error
        if (scale == 0)
        {
            input = 0.00001f;
        }
        //transform the vector by the global offset
        input = input - globalOffset;
        input = (input - pivot) * (1 / scale) + pivot;
        input = input - localOffset;
        return input;
    }
}