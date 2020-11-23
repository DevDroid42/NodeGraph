public class Evaluable
{
    //offset applied pre-scaling
    Evaluable localOffset;
    //offset applied post-scaling
    Evaluable globalOffset;
    Evaluable scale;
    Evaluable rot;
    Evaluable pivot;

    public virtual ColorVec EvaluateColor(float x, float y, float z, float w)
    {
        return new ColorVec(0);
    }

    public virtual float EvaluateValue(float x, float y, float z, float w)
    {
        return 0;
    }

    //used to get the resolution of the Evaluable data type.
    public virtual ColorVec GetResolution()
    {
        return 0;
    }

    public virtual void SetTransform(Evaluable localOffset, Evaluable globalOffset, Evaluable scale, Evaluable rot, Evaluable pivot)
    {
        this.localOffset = localOffset;
        this.globalOffset = globalOffset;
        this.scale = scale;
        this.rot = rot;
        this.pivot = pivot;
    }

    //used to get copy of Evaluable object so references arn't being passed between nodes. Reference passing would entangle
    //node states and create problems
    public virtual Evaluable GetCopy()
    {
        return new Evaluable();
    }

    //to be used for vector transformations in subclasses
    protected ColorVec TransformVector(ColorVec vector)
    {
        //NOT IMPLEMENTED
        return new ColorVec(0,0,0,0);
    }
}