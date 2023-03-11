
using Newtonsoft.Json;

public class EvaluableFloat : IEvaluable
{
    [JsonProperty]
    private float num;

    public EvaluableFloat(float num)
    {
        this.num = num;
    }

    public void SetNum(float number)
    {
        num = number;
    }

    public ColorVec EvaluateColor(float vector)
    {
        return num;
    }

    public float EvaluateValue(float vector)
    {
        return num;
    }

    public object GetCopy()
    {
        return new EvaluableFloat(num);
    }

    public override string ToString()
    {
        return num.ToString();
    }

    public int GetResolution()
    {
        return 1;
    }
}
