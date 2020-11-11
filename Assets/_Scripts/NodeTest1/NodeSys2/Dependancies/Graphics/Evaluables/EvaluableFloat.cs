
public class EvaluableFloat : Evaluable
{
    private float num;

    public EvaluableFloat(float num)
    {
        this.num = num;
    }

    public void SetNum(float number)
    {
        num = number;
    }

    public override ColorVec EvaluateColor(float x, float y, float z, float w)
    {
        return num;
    }

    public override float EvaluateValue(float x, float y, float z, float w)
    {
        return num;
    }

    public override string ToString()
    {
        return "eFloat: " + num;
    }
}
