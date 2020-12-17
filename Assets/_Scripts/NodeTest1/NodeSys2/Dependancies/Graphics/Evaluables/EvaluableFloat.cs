
using Newtonsoft.Json;

public class EvaluableFloat : Evaluable
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

    public override ColorVec EvaluateColor(float x, float y, float z, float w)
    {
        return num;
    }

    public override float EvaluateValue(float x, float y, float z, float w)
    {
        return num;
    }

    public override Evaluable GetCopy()
    {
        return new EvaluableFloat(num);
    }

    public override string ToString()
    {
        return num.ToString();
    }
}
