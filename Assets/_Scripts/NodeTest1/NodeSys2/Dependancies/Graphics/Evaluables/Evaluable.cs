public class Evaluable
{
    Evaluable offset;
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

    //to be used for vector transformations in subclasses
    protected ColorVec transformVector(ColorVec vector)
    {
        //NOT IMPLEMENTED
        return new ColorVec(0,0,0,0);
    }
}