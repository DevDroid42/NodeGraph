using UnityEngine;
public class Evaluable
{
    //offset applied pre-scaling
    public ColorVec localOffset = 0;
    //offset applied post-scaling
    public ColorVec globalOffset = 0;
    public ColorVec scale = 1;
    public ColorVec rot = 0;
    public ColorVec pivot = 0;    

    //Note: should always return a new color, not one with a reference inside the evaluable object
    public virtual ColorVec EvaluateColor(ColorVec vector)
    {
        return new ColorVec(0);
    }

    public virtual float EvaluateValue(ColorVec vector)
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
    public virtual Evaluable GetCopy()
    {
        //Debug.LogWarning("WARNING: Using Default get copy, this should never be run");
        return new Evaluable();
    }

    //to be used for vector transformations in subclasses
    protected ColorVec TransformVector(ColorVec vector)
    {        
        for (int i = 0; i < 4; i++)
        {
            if (scale.getComponent(i) == 0)
            {
                vector.SetComponent(i, 0.00001f);
                continue;
            }
            //transform the vector by the global offset
            vector.SetComponent(i, vector.getComponent(i) - globalOffset.getComponent(i));
            float pivotVal = pivot.getComponent(i);
            vector.SetComponent(i, (vector.getComponent(i) - pivotVal) * 1 / scale.getComponent(i) + pivotVal);
            vector.SetComponent(i, vector.getComponent(i) - localOffset.getComponent(i));
        }
        return vector;
    }
}